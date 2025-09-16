using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public static class PokerEvaluator
{
    // 牌型评估结果
    public class PokerEvaluationResult {
        public PokerHand handType; // 牌型：如 Pair, Straight, Flush...
        public List<CardData> relevantCards; // 关键牌（如：对子牌）
        public List<CardData> kickerCards;   // 踢脚牌（用于平局比较）

        public PokerEvaluationResult(PokerHand type, List<CardData> relevant = null, List<CardData> kicker = null) {
            handType = type;
            relevantCards = relevant ?? new List<CardData>();
            kickerCards = kicker ?? new List<CardData>();
        }
    }

    public static PokerEvaluationResult EvaluateHand(List<CardData> originalCards) {
        List<CardData> cards = new List<CardData>(originalCards);
        cards.Sort((a, b) => b.rank.CompareTo(a.rank));
        // ✅ 优先级修正：先检查同花顺（最高）
        if (IsStraightFlush(cards, out var straightFlush)) {
            return new PokerEvaluationResult(PokerHand.同花顺, straightFlush);
        }
        if (IsThreeOfAKind(cards, out var three)) {
            return new PokerEvaluationResult(PokerHand.三条, three);
        }
        if (IsFlush(cards, out var flush)) {
            return new PokerEvaluationResult(PokerHand.同花, flush);
        }
        if (IsStraight(cards, out var straight)) {
            return new PokerEvaluationResult(PokerHand.顺子, straight);
        }
        if (IsOnePair(cards, out var pair)) {
            var kickers = GetKickerCards(cards, pair);
            return new PokerEvaluationResult(PokerHand.一对, pair, kickers);
        }
        return new PokerEvaluationResult(PokerHand.高牌, new List<CardData> { cards[0] }, cards.Skip(1).ToList());
    }

    // --- Helper Functions ---
    private static bool IsFlush(List<CardData> cards, out List<CardData> flushResult) {
        var suitGroups = cards.GroupBy(c => c.suit).Where(g => g.Count() >= 3).ToList();
        if (suitGroups.Count > 0) {
            var sorted = suitGroups[0].OrderByDescending(c => c.rank).ToList();
            flushResult = sorted.Take(3).ToList();
            return true;
        }
        flushResult = null;
        return false;
    }

    private static bool IsStraight(List<CardData> cards, out List<CardData> straightResult) {
        var uniqueRanks = cards.Select(c => c.rank).Distinct().ToList();

        // ✅ 验证 A-2-3 是否存在
        if (uniqueRanks.Contains(14) && uniqueRanks.Contains(2) && uniqueRanks.Contains(3)) 
        {
            straightResult = new List<CardData> {
                cards.FirstOrDefault(c => c.rank == 3),
                cards.FirstOrDefault(c => c.rank == 2),
                cards.FirstOrDefault(c => c.rank == 14)
            };
            return true;
        }

        // ✅ 检查普通顺子
        var sortedRanks = uniqueRanks.OrderByDescending(x => x).ToList();
        for (int i = 0; i <= sortedRanks.Count - 3; i++) 
        {
            if (sortedRanks[i] - sortedRanks[i + 2] == 2) 
            {
                var straightRanks = uniqueRanks.Skip(i).Take(3).ToList();
                straightResult = cards.Where(c => straightRanks.Contains(c.rank)).OrderByDescending(c => c.rank).ToList();
                return true;
            }
        }

        straightResult = null;
        return false;
    }
    
    private static bool IsThreeOfAKind(List<CardData> cards, out List<CardData> threeOfAKindResult) {
        var group = cards.GroupBy(c => c.rank).FirstOrDefault(g => g.Count() == 3);
        if (group != null) {
            threeOfAKindResult = group.OrderByDescending(c => c.rank).ToList();
            return true;
        }
        threeOfAKindResult = null;
        return false;
    }


    private static bool IsOnePair(List<CardData> cards, out List<CardData> pairResult) {
        var group = cards.GroupBy(c => c.rank).FirstOrDefault(g => g.Count() == 2);
        if (group != null) {
            pairResult = group.OrderByDescending(c => c.rank).ToList();
            return true;
        }
        pairResult = null;
        return false;
    }
    
    private static bool IsStraightFlush(List<CardData> cards, out List<CardData> straightResult)
    {
        var uniqueRanks = cards.Select(c => c.rank).Distinct().ToList();

        // 处理 A-2-3 顺子（A视为1）
        if (uniqueRanks.Contains(14) && uniqueRanks.Contains(2) && uniqueRanks.Contains(3))
        {
            var candidate = new List<CardData> {
                cards.FirstOrDefault(c => c.rank == 3),
                cards.FirstOrDefault(c => c.rank == 2),
                cards.FirstOrDefault(c => c.rank == 14)
            };
            // ✅ 检查是否同花色
            if (candidate.All(c => c.suit == candidate[0].suit)) {
                straightResult = candidate;
                return true;
            }
        }

        // 检查普通顺子并确保同花色
        var sortedRanks = uniqueRanks.OrderByDescending(x => x).ToList();
        for (int i = 0; i <= sortedRanks.Count - 3; i++)
        {
            int highRank = sortedRanks[i];
            int midRank = sortedRanks[i + 1];
            int lowRank = sortedRanks[i + 2];

            if (highRank - lowRank == 2)
            {
                var straightRanks = new List<int> { highRank, midRank, lowRank };
                var candidates = cards.Where(c => straightRanks.Contains(c.rank)).ToList();
                // ✅ 验证是否同花色
                if (candidates.GroupBy(c => c.suit).Count() == 1) {
                    straightResult = candidates;
                    return true;
                }
            }
        }

        straightResult = null;
        return false;
    }

    
    private static List<CardData> GetKickerCards(List<CardData> fullHand, List<CardData> usedCards) {
        return fullHand.Where(c => !usedCards.Any(u => u.rank == c.rank && u.suit == c.suit)).ToList();
    }
}


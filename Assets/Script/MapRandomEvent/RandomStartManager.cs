using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Tilemaps;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

/// <summary>
/// 场景中随机刷新流星
/// 1.随机获取地图范围内坐标点
/// 2.流星落地点加载预告动画，流星播放出现动画
/// 3.流星到达落地点之后，停留指定时间消失
/// </summary>
public class RandomStartManager : MonoBehaviour
{
	[Header("随机流星数据")] 
	public int starCount_player = 2;
	public int starCount_camera = 2;
	public float starLandTime = 2f;
	public float starTime = 2f;
	public float starWaveTime = 5f;
	private float timer = 0;
	public GameObject Map;
	public GameObject star_land;
	public GameObject star;
	private Tilemap tilemap;
	private List<Vector3> validPoints = new List<Vector3>();
	public Transform startPosition;

	public Player player;

	[Header("相机参数")] 
	public Camera mainCamera; // 可通过 Inspector 赋值，也可自动获取主相机

	private void OnEnable()
	{
		if (mainCamera == null)
		{
			mainCamera = Camera.main; // 自动查找主相机
		}
	}

	public void CollectValidPoints()
	{
		validPoints.Clear(); // 清空旧数据
		
		RoomController roomController = Map.transform.GetChild(0).GetComponent<RoomController>();
		tilemap = roomController.bgTilemap;
		BoundsInt bounds = tilemap.cellBounds;
		TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

		for (int x = 0; x < bounds.size.x; x++)
		{
			for (int y = 0; y < bounds.size.y; y++)
			{
				// 获取瓦片本地坐标
				Vector3Int localCellPos = new Vector3Int(
					bounds.xMin + x,
					bounds.yMin + y,
					0
				);

				// 如果该位置有瓦片铺放
				if (tilemap.HasTile(localCellPos))
				{
					// 转换为世界坐标（单元格中心）
					Vector3 worldPos = tilemap.CellToWorld(localCellPos) + tilemap.transform.position;
					worldPos += tilemap.cellSize * 0.5f;

					validPoints.Add(worldPos);
				}
			}
		}

		Debug.Log($"✅ 找到 {validPoints.Count} 个有效点");
		// Debug.Log($"✅ 找到有效点: {GetRandomValidPoint()}");
	}
	private void Update()
	{
		if (timer <= starWaveTime)
		{
			timer += Time.deltaTime;
		}
		else
		{
			timer = 0;
			SetRandomStar();
		}
	}

	public void SetRandomStar()
	{
		List<Vector3> cameraPoints = GetValidPointsInCameraView();
		List<Vector3> playerPoints = GetValidPointsInPlayerView();

		if (cameraPoints.Count == 0)
		{
			Debug.LogWarning("⚠️ 相机视野内没有有效点！");
			return;
		}
		if (cameraPoints.Count == 0)
		{
			Debug.LogWarning("⚠️ 角色视野内没有有效点！");
			return;
		}

		for (int i = 0; i < starCount_camera && i < cameraPoints.Count; i++)
		{
			int randomIndex = Random.Range(0, cameraPoints.Count);
			Vector3 starPos = cameraPoints[randomIndex];

			GameObject starInstance = Instantiate(star_land, starPos, Quaternion.identity);
			Destroy(starInstance, starLandTime);
			StartCoroutine(LoadStar(starPos));
		}
		
		for (int i = 0; i < starCount_player && i < playerPoints.Count; i++)
		{
			int randomIndex = Random.Range(0, playerPoints.Count);
			Vector3 starPos = playerPoints[randomIndex];

			GameObject starInstance = Instantiate(star_land, starPos, Quaternion.identity);
			Destroy(starInstance, starLandTime);
			StartCoroutine(LoadStar(starPos));
		}
	}

	IEnumerator LoadStar(Vector3 pos)
	{
		yield return new WaitForSeconds(1.1f);
		var starObj = Instantiate(star, startPosition.position, Quaternion.identity, transform);
		starObj.transform.DOMove(pos, 0.5f).SetAutoKill(true).SetLink(starObj.gameObject);
		StartCoroutine(DeleteStar(starObj));
	}
	IEnumerator DeleteStar(GameObject starObj)
	{
		yield return new WaitForSeconds(starTime);
		if (starObj != null)
			Destroy(starObj, starTime);
	}

	private List<Vector3> GetValidPointsInPlayerView()
	{
		List<Vector3> cameraPoints = new List<Vector3>();

		foreach (Vector3 point in validPoints)
		{
			float distance = Vector3.Distance(player.transform.position, point);
			if(distance<=1.3f)
				cameraPoints.Add(point);
		}

		return cameraPoints;
	}
	private List<Vector3> GetValidPointsInCameraView()
	{
		List<Vector3> cameraPoints = new List<Vector3>();
		Bounds camBounds = GetCameraBounds();

		foreach (Vector3 point in validPoints)
		{
			if (camBounds.Contains(point))
			{
				cameraPoints.Add(point);
			}
		}

		return cameraPoints;
	}

	private Bounds GetCameraBounds()
	{
		float camHeight = mainCamera.orthographicSize;
		float camWidth = camHeight * mainCamera.aspect;

		Vector3 center = mainCamera.transform.position;

		Bounds bounds = new Bounds
		{
			center = center,
			extents = new Vector3(camWidth, camHeight, 100)
		};

		return bounds;
	}

}

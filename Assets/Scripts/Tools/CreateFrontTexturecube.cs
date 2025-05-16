using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateFrontTexturecube : MonoBehaviour
{
    [MenuItem("Tools/Create Front Textured Cube Mesh")]
    static void CreateCubeMesh()
    {
        // 메시 생성
        Mesh mesh = new Mesh();
        mesh.name = "FrontTexturedCube";

        // 정점 생성
        Vector3[] vertices = {
            // 앞면 (0-3)
            new Vector3(-0.5f, -0.5f, 0.5f),
            new Vector3(0.5f, -0.5f, 0.5f),
            new Vector3(0.5f, 0.5f, 0.5f),
            new Vector3(-0.5f, 0.5f, 0.5f),
            // 뒷면 (4-7)
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, 0.5f, -0.5f),
            new Vector3(-0.5f, 0.5f, -0.5f),
            // 왼쪽면 (8-11)
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(-0.5f, -0.5f, 0.5f),
            new Vector3(-0.5f, 0.5f, 0.5f),
            new Vector3(-0.5f, 0.5f, -0.5f),
            // 오른쪽면 (12-15)
            new Vector3(0.5f, -0.5f, 0.5f),
            new Vector3(0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, 0.5f, -0.5f),
            new Vector3(0.5f, 0.5f, 0.5f),
            // 윗면 (16-19)
            new Vector3(-0.5f, 0.5f, 0.5f),
            new Vector3(0.5f, 0.5f, 0.5f),
            new Vector3(0.5f, 0.5f, -0.5f),
            new Vector3(-0.5f, 0.5f, -0.5f),
            // 아랫면 (20-23)
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, -0.5f, 0.5f),
            new Vector3(-0.5f, -0.5f, 0.5f)
        };

        // 법선 벡터 직접 정의
        Vector3[] normals = new Vector3[vertices.Length];

        // 앞면 법선
        for (int i = 0; i < 4; i++)
            normals[i] = Vector3.forward;

        // 뒷면 법선
        for (int i = 4; i < 8; i++)
            normals[i] = Vector3.back;

        // 왼쪽면 법선
        for (int i = 8; i < 12; i++)
            normals[i] = Vector3.left;

        // 오른쪽면 법선
        for (int i = 12; i < 16; i++)
            normals[i] = Vector3.right;

        // 윗면 법선
        for (int i = 16; i < 20; i++)
            normals[i] = Vector3.up;

        // 아랫면 법선
        for (int i = 20; i < 24; i++)
            normals[i] = Vector3.down;

        // 삼각형 인덱스 정의
        int[] triangles = {
            // 앞면
            0, 1, 2,
            0, 2, 3,
            // 뒷면
            4, 6, 5,
            4, 7, 6,
            // 왼쪽면
            8, 9, 10,
            8, 10, 11,
            // 오른쪽면
            12, 13, 14,
            12, 14, 15,
            // 윗면
            16, 17, 18,
            16, 18, 19,
            // 아랫면
            20, 21, 22,
            20, 22, 23
        };

        // UV 좌표 설정
        Vector2[] uv = new Vector2[vertices.Length];
        for (int i = 0; i < vertices.Length / 4; i++)
        {
            uv[i * 4 + 0] = new Vector2(0, 0);
            uv[i * 4 + 1] = new Vector2(1, 0);
            uv[i * 4 + 2] = new Vector2(1, 1);
            uv[i * 4 + 3] = new Vector2(0, 1);
        }

        // 메시 데이터 적용
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uv;

        // 서브메시 설정
        mesh.subMeshCount = 2;

        // 앞면에 대한 삼각형 인덱스
        int[] frontTriangles = { 0, 1, 2, 0, 2, 3 };
        mesh.SetTriangles(frontTriangles, 0);

        // 다른 면들에 대한 삼각형 인덱스
        int[] otherTriangles = new int[triangles.Length - 6];
        System.Array.Copy(triangles, 6, otherTriangles, 0, triangles.Length - 6);
        mesh.SetTriangles(otherTriangles, 1);

        // 에셋으로 저장
        string path = "Assets/Meshes";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        AssetDatabase.CreateAsset(mesh, path + "/FrontTexturedCube.asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Front Textured Cube Mesh created at " + path + "/FrontTexturedCube.asset");

        // 선택된 상태로 만들기
        Selection.activeObject = mesh;
    }
}

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
        // �޽� ����
        Mesh mesh = new Mesh();
        mesh.name = "FrontTexturedCube";

        // ���� ����
        Vector3[] vertices = {
            // �ո� (0-3)
            new Vector3(-0.5f, -0.5f, 0.5f),
            new Vector3(0.5f, -0.5f, 0.5f),
            new Vector3(0.5f, 0.5f, 0.5f),
            new Vector3(-0.5f, 0.5f, 0.5f),
            // �޸� (4-7)
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, 0.5f, -0.5f),
            new Vector3(-0.5f, 0.5f, -0.5f),
            // ���ʸ� (8-11)
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(-0.5f, -0.5f, 0.5f),
            new Vector3(-0.5f, 0.5f, 0.5f),
            new Vector3(-0.5f, 0.5f, -0.5f),
            // �����ʸ� (12-15)
            new Vector3(0.5f, -0.5f, 0.5f),
            new Vector3(0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, 0.5f, -0.5f),
            new Vector3(0.5f, 0.5f, 0.5f),
            // ���� (16-19)
            new Vector3(-0.5f, 0.5f, 0.5f),
            new Vector3(0.5f, 0.5f, 0.5f),
            new Vector3(0.5f, 0.5f, -0.5f),
            new Vector3(-0.5f, 0.5f, -0.5f),
            // �Ʒ��� (20-23)
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, -0.5f, 0.5f),
            new Vector3(-0.5f, -0.5f, 0.5f)
        };

        // ���� ���� ���� ����
        Vector3[] normals = new Vector3[vertices.Length];

        // �ո� ����
        for (int i = 0; i < 4; i++)
            normals[i] = Vector3.forward;

        // �޸� ����
        for (int i = 4; i < 8; i++)
            normals[i] = Vector3.back;

        // ���ʸ� ����
        for (int i = 8; i < 12; i++)
            normals[i] = Vector3.left;

        // �����ʸ� ����
        for (int i = 12; i < 16; i++)
            normals[i] = Vector3.right;

        // ���� ����
        for (int i = 16; i < 20; i++)
            normals[i] = Vector3.up;

        // �Ʒ��� ����
        for (int i = 20; i < 24; i++)
            normals[i] = Vector3.down;

        // �ﰢ�� �ε��� ����
        int[] triangles = {
            // �ո�
            0, 1, 2,
            0, 2, 3,
            // �޸�
            4, 6, 5,
            4, 7, 6,
            // ���ʸ�
            8, 9, 10,
            8, 10, 11,
            // �����ʸ�
            12, 13, 14,
            12, 14, 15,
            // ����
            16, 17, 18,
            16, 18, 19,
            // �Ʒ���
            20, 21, 22,
            20, 22, 23
        };

        // UV ��ǥ ����
        Vector2[] uv = new Vector2[vertices.Length];
        for (int i = 0; i < vertices.Length / 4; i++)
        {
            uv[i * 4 + 0] = new Vector2(0, 0);
            uv[i * 4 + 1] = new Vector2(1, 0);
            uv[i * 4 + 2] = new Vector2(1, 1);
            uv[i * 4 + 3] = new Vector2(0, 1);
        }

        // �޽� ������ ����
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uv;

        // ����޽� ����
        mesh.subMeshCount = 2;

        // �ո鿡 ���� �ﰢ�� �ε���
        int[] frontTriangles = { 0, 1, 2, 0, 2, 3 };
        mesh.SetTriangles(frontTriangles, 0);

        // �ٸ� ��鿡 ���� �ﰢ�� �ε���
        int[] otherTriangles = new int[triangles.Length - 6];
        System.Array.Copy(triangles, 6, otherTriangles, 0, triangles.Length - 6);
        mesh.SetTriangles(otherTriangles, 1);

        // �������� ����
        string path = "Assets/Meshes";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        AssetDatabase.CreateAsset(mesh, path + "/FrontTexturedCube.asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Front Textured Cube Mesh created at " + path + "/FrontTexturedCube.asset");

        // ���õ� ���·� �����
        Selection.activeObject = mesh;
    }
}

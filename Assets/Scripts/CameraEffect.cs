using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraEffect : MonoBehaviour
{
	public Material Material;

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (Material == null)
		{
			Graphics.Blit(source, destination);
			return;
		}

		Graphics.Blit(source, destination, Material);
	}
}
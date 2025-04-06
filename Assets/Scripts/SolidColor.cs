using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidColor : MonoBehaviour
{

    public ComputeShader shader;
    public int texResolution = 256;
    public string kernelName = "SolidRed";
    Renderer rend;
    RenderTexture outputTexture;
    int kernelHandle;

    // Start is called before the first frame update
    void Start()
    {
        outputTexture = new RenderTexture(texResolution, texResolution, 0);
        outputTexture.enableRandomWrite = true;
        outputTexture.Create();

        rend = GetComponent<Renderer>();
        rend.enabled = true;

        InitShader();
    }

    private void InitShader() {
        kernelHandle = shader.FindKernel(kernelName);

        int halfRes = texResolution >> 1;
        int quartRes = texResolution >> 2;

        Vector4 rect = new Vector4(quartRes, quartRes, halfRes, halfRes); // x = left, y = bottom, z = width, w = height of the square
        shader.SetVector("rect", rect);

        shader.SetInt("texResolution", texResolution);
        shader.SetTexture(kernelHandle, "Result", outputTexture);
        rend.material.SetTexture("_MainTex", outputTexture);

        DispatchShader(texResolution / 8, texResolution / 8);
    }

    private void DispatchShader(int x, int y) {
        shader.Dispatch(kernelHandle, x, y, 1);
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.U)) {
            DispatchShader(texResolution / 8, texResolution / 8);
        }
    }
}

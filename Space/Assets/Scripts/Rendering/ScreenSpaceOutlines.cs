using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ScreenSpaceOutlines :ScriptableRendererFeature
{
    private class ViewSpaceNormalsTexturePass :
        ScriptableRenderPass{ }

    private class ScreenSpaceOutlinePass :
        ScriptableRenderPass{ }

    public override void Create()
    {
       
    }
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        throw new System.NotImplementedException();
    }
}

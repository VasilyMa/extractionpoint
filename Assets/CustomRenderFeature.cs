using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class CustomRenderFeature : ScriptableRendererFeature
{
    class CustomRenderPass : ScriptableRenderPass
    {
        private string shaderPassName = "CustomPass"; // ��� pass ������ �������
        private Material material;

        public CustomRenderPass(Material material)
        {
            this.material = material;
            renderPassEvent = RenderPassEvent.AfterRenderingOpaques; // ��� �������� ���������� ��� ������ ������
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var cmd = CommandBufferPool.Get("CustomRenderPass");

            // ��������� DrawingSettings ��� ������ �������
            DrawingSettings drawingSettings = CreateDrawingSettings(new ShaderTagId(shaderPassName), ref renderingData, SortingCriteria.CommonOpaque);
            FilteringSettings filteringSettings = new FilteringSettings(RenderQueueRange.opaque);

            // ���������� ���������
            context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref filteringSettings);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    public Material material; // �������� ������ �������
    CustomRenderPass customRenderPass;

    public override void Create()
    {
        if (material == null)
        {
            Debug.LogError("���������, ��� �������� �������� � CustomRenderFeature!");
            return;
        }

        customRenderPass = new CustomRenderPass(material);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(customRenderPass);
    }
}

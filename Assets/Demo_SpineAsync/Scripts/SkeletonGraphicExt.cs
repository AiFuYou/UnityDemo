using System.Threading.Tasks;
using Spine;
using Spine.Unity;

public class SkeletonGraphicExt : SkeletonGraphic
{
    public async Task<bool> InitializeAsync(bool overwrite = false)
    {
        if (this.IsValid && !overwrite) return true;
        
        SetMeshBuffers(new DoubleBuffered<MeshRendererBuffers.SmartMesh>());
        
        // Make sure none of the stuff is null

        await Task.Run(() => { 
            if (this.skeletonDataAsset == null) return;
            var skeletonData = this.skeletonDataAsset.GetSkeletonData(false);
            if (skeletonData == null) return;
				
            if (skeletonDataAsset.atlasAssets.Length <= 0 || skeletonDataAsset.atlasAssets[0].materials.Length <= 0) return;

            this.state = new Spine.AnimationState(skeletonDataAsset.GetAnimationStateData());
            if (state == null) {
                Clear();
                return;
            }
				
            this.skeleton = new Skeleton(skeletonData) {
                flipX = this.initialFlipX,
                flipY = this.initialFlipY
            };

            MainThread.RunTask(() =>
            {
                canvasRenderer.SetTexture(this.mainTexture); // Needed for overwriting initializations.
            });
				
            // Set the initial Skin and Animation
            if (!string.IsNullOrEmpty(initialSkinName))
                skeleton.SetSkin(initialSkinName);
        });

        return true;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Spine.Unity;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SpineTest : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        var spineTestGameObject = transform.Find("spineTest").gameObject;
        spineTestGameObject.SetActive(false);
        var spineTestSkeletonExt = spineTestGameObject.AddComponent<SkeletonGraphicExt>();
        var mat = Addressables.LoadAssetAsync<Material>("spineTestMat");
        var data = Addressables.LoadAssetAsync<SkeletonDataAsset>("spineTestData");

        while (!mat.IsDone || !data.IsDone)
        {
            await Task.Delay(1);
        }

        if (data.Status == AsyncOperationStatus.Succeeded)
        {
            spineTestSkeletonExt.skeletonDataAsset = data.Result;
        }
                
        if (mat.Status == AsyncOperationStatus.Succeeded)
        {
            spineTestSkeletonExt.material = mat.Result;
        }

        await spineTestSkeletonExt.InitializeAsync();

        spineTestGameObject.SetActive(true);
        spineTestSkeletonExt.AnimationState.SetAnimation(0, "daiji1", true);
    }
}

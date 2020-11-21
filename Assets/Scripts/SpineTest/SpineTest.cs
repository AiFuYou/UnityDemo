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
        var mat = await Addressables.LoadAssetAsync<Material>("spineTestMat").Task;
        var data = await Addressables.LoadAssetAsync<SkeletonDataAsset>("spineTestData").Task;

        spineTestSkeletonExt.skeletonDataAsset = data;
        spineTestSkeletonExt.material = mat;

        await spineTestSkeletonExt.InitializeAsync();

        spineTestGameObject.SetActive(true);
        spineTestSkeletonExt.AnimationState.SetAnimation(0, "daiji1", true);
    }
}

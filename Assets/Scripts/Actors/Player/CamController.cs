using UnityEngine;

public class CamController : MonoBehaviour
{
    public PlayerCharacter target;

    [Header("--- FOW Setup ---")]
    [SerializeField] GameObject sceneFOWSource;
    [SerializeField] GameObject gameFOWSource;

    [Header("--- VFX ---")]
    [SerializeField] Material dmgScreenVFX;
    [SerializeField] GameObject background;

    [Header("--- Recoil Config ---")]
    [Range(0f, 1f)] public float recoverSpeed = 0.125f;
    [Range(1, 10)] public float recoilMultiplier = 2f;
    private Vector2 recoiledPos = Vector2.zero;
    private float originalRecoverSpeed;

    float elapsedTime = 0f;
    float projectedDuration = 0f;

    enum RecoilStage
    {
        recovering = -1,
        innactive = 0,
        recoiling = 1,
    }

    RecoilStage currentStage = RecoilStage.innactive;

    void Start()
    {
        originalRecoverSpeed = recoverSpeed;
        sceneFOWSource.SetActive(false);
        gameFOWSource.SetActive(true);
    }

    void FixedUpdate()
    {
        if (currentStage != RecoilStage.innactive)
        {
            StepRecoil();
            //background.transform.position = this.transform.position;
        }
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (dmgScreenVFX != null)
        {
            Graphics.Blit(source, destination, dmgScreenVFX);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }

    public void StartRecoil(float strength, Vector2 lookDir)
    {
        Vector2 recoilDir = -lookDir;

        recoiledPos += (Vector2)this.transform.TransformVector(recoilDir * (strength / 2));
        LimitRecoil(strength, recoilDir);

        elapsedTime = 0f;
        recoverSpeed = originalRecoverSpeed * recoilMultiplier;
        projectedDuration = recoiledPos.magnitude / recoverSpeed;
        currentStage = RecoilStage.recoiling;
    }

    void LimitRecoil(float strength, Vector2 recoilDir)
    {
        bool isAboveMaxRecoil = recoiledPos.magnitude > ((recoilDir * strength).magnitude * 2);
        if (isAboveMaxRecoil)
        {
            recoiledPos = (Vector2)this.transform.TransformVector((recoilDir * strength) * 2);
        }
    }

    private void StepRecoil()
    {
        elapsedTime += Time.fixedDeltaTime;
        Vector2 recoilStep = Vector2.Lerp((Vector2)transform.localPosition, recoiledPos, recoverSpeed);
        transform.localPosition = (Vector3)recoilStep;

        bool finished = Vector2.Distance(transform.localPosition, recoiledPos) <= 0.01f;
        if (elapsedTime >= projectedDuration || finished)
        {
            if (currentStage == RecoilStage.recoiling)
            {
                recoverSpeed = originalRecoverSpeed;
                currentStage = RecoilStage.recovering;
                elapsedTime = 0f;
                projectedDuration = recoiledPos.magnitude / recoverSpeed;
                transform.localPosition = recoiledPos;
                recoiledPos = Vector2.zero;
            }
            else
            {
                recoiledPos = Vector2.zero;
                this.transform.localPosition = recoiledPos;
                currentStage = RecoilStage.innactive;
                transform.localPosition = recoiledPos;
            }
        }
    }

}

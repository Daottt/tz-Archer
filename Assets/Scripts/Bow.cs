using UnityEngine;
using Spine.Unity;

public class Bow : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private SkeletonAnimation skeletonAnimation;
    [SpineBone(dataField: "skeletonAnimation")]
    [SerializeField] public string rotatingBoneName;
    [SerializeField] private Transform ArrowPoint;
    [SerializeField] private Arrow ArrowPrefab;

    [SerializeField] private float ArrowSpeed;
    [SerializeField] private int linePoints = 25;
    [SerializeField] private float timeBetweenPoints = 0.1f;
    private LineRenderer lineRenderer;
    private Vector3 startMousePos, direction;
    private Spine.Bone bone;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        bone = skeletonAnimation.Skeleton.FindBone(rotatingBoneName);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startMousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            lineRenderer.enabled = true;
            lineRenderer.positionCount = Mathf.CeilToInt(linePoints / timeBetweenPoints) + 1;
            skeletonAnimation.AnimationState.SetAnimation(0, "attack_start", false);
            skeletonAnimation.AnimationState.AddAnimation(0, "attack_target", false, 0);
        }

        if (Input.GetMouseButton(0))
        {
            direction = startMousePos - mainCamera.ScreenToWorldPoint(Input.mousePosition);
            DrawTrajectory();
            var rotate = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            bone.Rotation = rotate;
            skeletonAnimation.Skeleton.UpdateWorldTransform();
        }

        if (Input.GetMouseButtonUp(0))
        {
            Shoot();
            lineRenderer.enabled = false;
            skeletonAnimation.AnimationState.SetAnimation(0, "attack_finish", false);
        }
    }

    private void DrawTrajectory()
    {
        lineRenderer.positionCount = Mathf.CeilToInt(linePoints / timeBetweenPoints) + 1;
        Vector2 startPosition = ArrowPoint.position;
        Vector2 startVelocity = ArrowSpeed * direction / 1;
        int i = 0;
        lineRenderer.SetPosition(i, startPosition);
        for (float time = 0; time < linePoints; time += timeBetweenPoints)
        {
            i++;
            Vector2 point = startPosition + time * startVelocity;
            point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);

            lineRenderer.SetPosition(i, point);
        }
    }

    private void Shoot()
    {
        var arrow = Instantiate(ArrowPrefab, ArrowPoint.position, ArrowPoint.rotation);
        arrow.Throw(ArrowSpeed, direction);
    }
}

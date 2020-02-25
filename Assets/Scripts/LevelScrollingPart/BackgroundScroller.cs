using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundScroller : BaseScroller
{
    public bool DrawGizmos;

    float _rightBound;
    float _leftBound;
    Vector3 _distanceBetweenBounds;

    // Start is called before the first frame update
    void Start()
    {
        base.RequestScrollSpeed();

        float xPos = this.transform.position.x;
        float distanceToBound = GetComponent<SpriteRenderer>().bounds.extents.x / 3f;

        _rightBound = xPos + distanceToBound;
        _leftBound = xPos - distanceToBound;

        _distanceBetweenBounds = new Vector3(_rightBound - _leftBound, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (base.CurrentScrollSpeed != 0f)
        {
            transform.localPosition += Vector3.left * base.CurrentScrollSpeed * Time.deltaTime;

            if (IsPassedBound)
                MoveToOppositeBound();
        }
    }

    bool IsPassedBound
    {
        get
        {
            bool movingLeft = base.CurrentScrollSpeed > 0f;
            float xPos = this.transform.position.x;

            return (movingLeft && xPos < _leftBound) || (!movingLeft && xPos > _rightBound);
        }
    }

    void MoveToOppositeBound()
    {
        if (base.CurrentScrollSpeed > 0f)
            transform.position += _distanceBetweenBounds;
        else
            transform.position -= _distanceBetweenBounds;
    }

    private void OnDrawGizmos()
    {
        if (DrawGizmos && Application.isPlaying)
        {
            Gizmos.DrawCube(new Vector3(_rightBound, this.transform.position.y, this.transform.position.z), new Vector3(0.1f, 2f, 0.1f));
            Gizmos.DrawCube(new Vector3(_leftBound, this.transform.position.y, this.transform.position.z), new Vector3(0.1f, 2f, 0.1f));
        }
    }
}
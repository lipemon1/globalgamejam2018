using UnityEngine;

public class NoisyCamera : MonoBehaviour
{
    public const float RandomRange = 9999f;

    public bool RandomizeSeed = true;
    public Vector3 Seed;

    [Header("Position")] public float PositionGeneralAmplitude = 1f;
    public float PositionGeneralSpeed = 1f;
    public float PositionGeneralFrequency = 1f;
    [Space] public Vector3 PositionAmplitude = Vector3.one;
    public Vector3 PositionSpeed = Vector3.one;
    public Vector3 PositionFrequency = Vector3.one;

    [Header("Rotation")] public float RotationGeneralAmplitude = 1f;
    public float RotationGeneralSpeed = 1f;
    public float RotationGeneralFrequency = 1f;
    [Space] public Vector3 RotationAmplitude = Vector3.one;
    public Vector3 RotationSpeed = Vector3.one;
    public Vector3 RotationFrequency = Vector3.one;

    private Vector3 _positionInput = Vector3.zero;
    private Vector3 _rotationInput = Vector3.zero;

    private Vector3 _initialLocalPosition;
    private Vector3 _initialEulerAngles;

    private void Start()
    {
        _initialLocalPosition = transform.position;
        _initialEulerAngles = transform.localEulerAngles;

        if (RandomizeSeed)
            Seed = new Vector3(Random.Range(-RandomRange, RandomRange), Random.Range(-RandomRange, RandomRange),
                Random.Range(-RandomRange, RandomRange));
    }

    private void Update()
    {
        transform.localPosition = _initialLocalPosition + GetNoise(PositionAmplitude, PositionGeneralAmplitude, PositionFrequency,
                                      PositionGeneralFrequency, PositionSpeed, PositionGeneralSpeed, ref _positionInput);

        transform.localEulerAngles = _initialEulerAngles + GetNoise(RotationAmplitude, RotationGeneralAmplitude, RotationFrequency,
                                         RotationGeneralFrequency, RotationSpeed, RotationGeneralSpeed, ref _rotationInput);
    }

    private Vector3 GetNoise(Vector3 axisAmplitude, float generalAmplitude, Vector3 axisFrequency, float generalFrequency,
        Vector3 axisSpeed, float generalSpeed, ref Vector3 input)
    {
        var x = (Mathf.PerlinNoise((input.x + Seed.x) * axisFrequency.x * generalFrequency, 0) * 2 - 1) * axisAmplitude.x *
                generalAmplitude;
        var y = (Mathf.PerlinNoise((input.y + Seed.y) * axisFrequency.y * generalFrequency, 0) * 2 - 1) * axisAmplitude.y *
                generalAmplitude;
        var z = (Mathf.PerlinNoise((input.z + Seed.z) * axisFrequency.z * generalFrequency, 0) * 2 - 1) * axisAmplitude.z *
                generalAmplitude;

        input.x += axisSpeed.x * generalSpeed * Time.deltaTime;
        input.y += axisSpeed.y * generalSpeed * Time.deltaTime;
        input.z += axisSpeed.z * generalSpeed * Time.deltaTime;

        return new Vector3(x, y, z);
    }
}
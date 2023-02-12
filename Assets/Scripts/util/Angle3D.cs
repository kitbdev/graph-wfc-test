using UnityEngine;

public struct Angle3D {
    [Range(0, 360f)]
    public float azimuth;
    [Range(-90, 90f)]
    public float altitude;


    float ClampAzimuth(float naz) {
        return naz % 360f;
    }
    float ClampAltitude(float nalt) {
        return Mathf.Clamp(nalt, -90, 90f);
    }

    public Angle3D Flip() {
        return new Angle3D() {
            azimuth = ClampAzimuth(azimuth - 360),
            altitude = ClampAltitude(-altitude),
        };
    }
    public Vector3 ToVector3() {
        return new Vector3(
            Mathf.Sin(azimuth) * Mathf.Cos(altitude),
            Mathf.Cos(azimuth) * Mathf.Sin(altitude),
            Mathf.Sin(altitude)
        );
    }
    public Quaternion ToQuaternion() {
        return Quaternion.Euler(altitude, azimuth, 0);
    }
}
public class Angle3DRange {
    public Angle3D min;
    public Angle3D max;
}
using UnityEngine;
using Unity.Mathematics;

public struct Angle3D {
    // longitude
    [Range(0, 360f)]
    public float azimuth;
    // latitude
    [Range(-90, 90f)]
    public float altitude;


    public static float WrapAzimuth(float naz) {
        return naz % 360f;
    }
    public static float ClampAltitude(float nalt) {
        return math.clamp(nalt, -90, 90f);
    }

    public Angle3D Flip() {
        return new Angle3D() {
            azimuth = WrapAzimuth(azimuth - 360),
            altitude = ClampAltitude(-altitude),
        };
    }
    // public Angle3D SignedDistanceTo(Angle3D other){
    //     float az = other.azimuth-this.azimuth;
    //     if (az>180) az -= 360;
    //     if (az<-180) az += 360;
    //     float alt = other.altitude-this.altitude;
    //     return 
    // }
    public float DistanceTo(Angle3D other) {
        //https://en.wikipedia.org/wiki/Great-circle_distance
        float centralAngle = math.acos(math.sin(this.altitude) * math.sin(other.altitude) + math.cos(this.altitude) * math.cos(other.altitude) * math.cos(math.abs(other.azimuth - this.azimuth)));
        float arcLength = math.radians(centralAngle) * 1;
        return arcLength;
    }

    public Vector3 ToVector3() {
        return new Vector3(
            math.sin(azimuth) * math.cos(altitude),
            math.cos(azimuth) * math.sin(altitude),
            math.sin(altitude)
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
using Hexed.Core;
using Hexed.SDK.Offsets;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using static Hexed.SDK.Offsets.EnumOffsets;

namespace Hexed.SDK.Engine
{
    internal class Structs
    {
        [StructLayout(LayoutKind.Explicit)]
        public struct FString
        {
            [FieldOffset(0x0)]
            public ulong pData;

            [FieldOffset(0x8)]
            public int DataSize;
            public override string ToString()
            {
                return Encoding.Unicode.GetString(GameManager.Memory.ReadByteArray(pData, DataSize * 4)).Split('\0')[0];
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct FTransform
        {
            [FieldOffset(StructOffsets.FTransform.Rotation)]
            public Quaternion Rotation;

            [FieldOffset(StructOffsets.FTransform.Translation)]
            public Vector3 Translation;

            [FieldOffset(StructOffsets.FTransform.Scale3D)]
            public Vector3 Scale3D;
        };

        [StructLayout(LayoutKind.Explicit)]
        public struct FActorId
        {
            [FieldOffset(StructOffsets.FActorId.ActorLocalName)]
            public FString ActorLocalName;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct FNetActorPtr
        {
            [FieldOffset(StructOffsets.FNetActorPtr.ObjectPtr)]
            public ulong ObjectPtr;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct FRepMovement
        {
            [FieldOffset(StructOffsets.FRepMovement.LinearVelocity)]
            public Vector3 LinearVelocity;

            [FieldOffset(StructOffsets.FRepMovement.AngularVelocity)]
            public Vector3 AngularVelocity;

            [FieldOffset(StructOffsets.FRepMovement.Location)]
            public Vector3 Location;

            [FieldOffset(StructOffsets.FRepMovement.Rotation)]
            public Vector3 Rotation;

            //[FieldOffset(StructOffsets.FRepMovement.bSimulatedPhysicSleep)]
            //public char bSimulatedPhysicSleep;

            //[FieldOffset(StructOffsets.FRepMovement.bRepPhysics)]
            //public char bRepPhysics;

            [FieldOffset(StructOffsets.FRepMovement.LocationQuantizationLevel)]
            public EVectorQuantization LocationQuantizationLevel;

            [FieldOffset(StructOffsets.FRepMovement.VelocityQuantizationLevel)]
            public EVectorQuantization VelocityQuantizationLevel;

            [FieldOffset(StructOffsets.FRepMovement.RotationQuantizationLevel)]
            public EVectorQuantization RotationQuantizationLevel;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct FTViewTarget
        {
            [FieldOffset(StructOffsets.FTViewTarget.Target)]
            public ulong Target;

            [FieldOffset(StructOffsets.FTViewTarget.POV)]
            public FMinimalViewInfo POV;

            [FieldOffset(StructOffsets.FTViewTarget.PlayerState)]
            public ulong PlayerState;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct FMinimalViewInfo
        {
            [FieldOffset(StructOffsets.FMinimalViewInfo.Location)]
            public Vector3 Location;

            [FieldOffset(StructOffsets.FMinimalViewInfo.Rotation)]
            public Vector3 Rotation;

            [FieldOffset(StructOffsets.FMinimalViewInfo.FOV)]
            public float FOV;

            [FieldOffset(StructOffsets.FMinimalViewInfo.OrthoWidth)]
            public float OrthoWidth;

            [FieldOffset(StructOffsets.FMinimalViewInfo.OrthoNearClipPlane)]
            public float OrthoNearClipPlane;

            [FieldOffset(StructOffsets.FMinimalViewInfo.OrthoFarClipPlane)]
            public float OrthoFarClipPlane;

            [FieldOffset(StructOffsets.FMinimalViewInfo.AspectRatio)]
            public float AspectRatio;

            //[FieldOffset(StructOffsets.FMinimalViewInfo.bConstrainAspectRatio)]
            //public char bConstrainAspectRatio;

            //[FieldOffset(StructOffsets.FMinimalViewInfo.bUseFieldOfViewForLOD)]
            //public char bUseFieldOfViewForLOD;

            [FieldOffset(StructOffsets.FMinimalViewInfo.ProjectionMode)]
            public ECameraProjectionMode ProjectionMode;

            [FieldOffset(StructOffsets.FMinimalViewInfo.PostProcessBlendWeight)]
            public float PostProcessBlendWeight;

            [FieldOffset(StructOffsets.FMinimalViewInfo.PostProcessSettings)]
            public ulong PostProcessSettings; // FPostProcessSettings but im lazy
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct FCurrentHealthInfo
        {
            [FieldOffset(StructOffsets.FCurrentHealthInfo.Health)]
            public float Health;

            [FieldOffset(StructOffsets.FCurrentHealthInfo.LastChangedReason)]
            public EHealthChangedReason LastChangedReason;

            [FieldOffset(StructOffsets.FCurrentHealthInfo.LastInstigatorLocation)]
            public Vector3 LastInstigatorLocation;

            [FieldOffset(StructOffsets.FCurrentHealthInfo.HealthChangeCount)]
            public int HealthChangeCount;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct FActorPtr
        {
            [FieldOffset(StructOffsets.FActorPtr.Actor)]
            public ulong Actor;
        }
    }
}

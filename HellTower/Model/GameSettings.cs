using System.Drawing;
using System.Windows.Forms;

namespace HellTower.Model
{
    public static class GameSettings
    {
        private static Rectangle screenBounds = Screen.PrimaryScreen.WorkingArea;
        public static int ScreenWidth => screenBounds.Width;
        public static int ScreenHeight => screenBounds.Height;

        public static float PlayerSpeed = 10f;
        public static float PlayerJumpForce = 30f;
        public static float Gravity = 2.95f;
        public static float DashDistance = 75f;
        public const float DashDuration = 0.2f;
        public const float DashCooldownDuration = 0.75f;
        public const float InvincibilityDuration = 0.5f;
        public static float HealDuration = 0.75f;

        public const int BlockWidth = 63;
        public const int BlockHeight = 45;

        public static int PlatformMinWidth = 378; 
        public static int PlatformMaxWidth = 819;  
        public static int PlatformHeight = BlockHeight;
        public static int PlatformMinVerticalDistance = 350;
        public static int PlatformMaxVerticalDistance = 450;

        public static int WindowMinVerticalDistance = 400;
        public static int WindowMaxVerticalDistance = 800;
        public static float BatSpawnChance = 0.02f; // 2% шанс спавна мыши из окна за кадр
        public static int MaxBatsFromWindow = 3;  
        public static float BatSpawnCooldown = 5f;

        public const float MaxHoldTime = 0.25f;
        public const float InitialJumpForce = -25f;
        public const float HoldJumpForce = -50f;

        public const float AnimationFrameTime = 0.1f; 

        public const float AttackKnockbackForce = 30f;
        public const float EnemyKnockbackForce = 10f;
        public const float KnockbackDuration = 0.3f;

        public const float AttackDuration = 0.15f;
        public const float AttackCooldownDuration = 0.15f;

        public const int MaxSkeletons = 5;
        public const float SkeletonSpawnCooldown = 5f;
        public const float SkeletonSpawnChance = 0.3f;
        public const float SkeletonMoveSpeed = 3f;
        public const float SkeletonDetectionRange = 300f;
        public const float SkeletonAttackRange = 135f;
        public const float SkeletonPatrolChangeTime = 2f;
        public const float SkeletonIdleDuration = 1f;
        public const float SkeletonAttackCooldownTime = 1.5f;
        public const float SkeletonAttackActiveTime = 0.15f;
        public const float SkeletonTimeToAttack = 0.2f;

        public const float BatSpeed = 5f;
        public const float BatDetectionRange = 800f;
        public const float BatMinDistance = 35f;
        public const float BatPatrolChangeTime = 0.4f;
        public const float BatPatrolSpeed = 3f;
    }
}

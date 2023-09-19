using UnityEngine;

namespace Source.Input
{
    public static class MousePositionCalculator
    {
        public static Vector2Int CalculateMousePosition(Vector2 screenPosition, Camera camera)
        {
            var mouseWorldPos = ConvertToWorldCoordinates(screenPosition, camera);
            var roundedWorldPos = new Vector2Int
            (
                Mathf.FloorToInt(mouseWorldPos.x),
                Mathf.FloorToInt(mouseWorldPos.y)
            );

            return roundedWorldPos;
        }

        private static Vector2 ConvertToWorldCoordinates(Vector2 screenPosition, Camera camera)
        {
            var mousePosAndDepthOfScreen = new Vector3(screenPosition.x, screenPosition.y, camera.transform.position.y);
            var mouseWorldPosition = camera.ScreenToWorldPoint(mousePosAndDepthOfScreen);
            var mouseWorldClippedPos = new Vector2(mouseWorldPosition.x / 10f, mouseWorldPosition.z / 10f);
            return mouseWorldClippedPos;
        }
    }
}

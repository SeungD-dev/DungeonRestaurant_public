using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    [SerializeField] private Image BGImage;
    [SerializeField] private Image TitleImage;
    [SerializeField] private Camera Camera;

    private float introTime = 3f;

    private void Start()
    {
        StartCoroutine(IntroStart(introTime, BGImage, TitleImage, Camera));
    }

    IEnumerator IntroStart(float time, Image bg, Image title, Camera camera)
    {
        Color bgColor = bg.color;
        Color titleColor = title.color;

        Vector3 startPosition = camera.transform.position;
        Vector3 targetPosition = new Vector3(0.2f,-0.9f , camera.transform.position.z);

        while (bgColor.a > 0.5f)
        {
            bgColor.a -= Time.deltaTime / time;
            bg.color = bgColor;
            yield return null;
        }

        SoundManager.Instance.PlaySound("BGM_Intro");
        while (titleColor.a < 1)
        {
            titleColor.a += Time.deltaTime / time;
            title.color = titleColor;
            yield return null;
        }

        float clickable = 0f;
        bool skipIntro = false;

        while (clickable < time)
        {
            if (Input.GetMouseButtonDown(0))
            {
                titleColor.a = 0f;
                bgColor.a = 0f;
                title.color = titleColor;
                bg.color = bgColor;
                skipIntro = true;
                break;
            }

            clickable += Time.deltaTime;
            yield return null;
        }
      
        if (!skipIntro)
        {
            yield return new WaitForSeconds(time - clickable);
        }

        while (titleColor.a > 0 || bgColor.a > 0)
        {
            if (titleColor.a > 0)
            {
                titleColor.a -= Time.deltaTime / time;
                title.color = titleColor;
            }

            if (bgColor.a > 0)
            {
                bgColor.a -= Time.deltaTime / time;
                bg.color = bgColor;
            }

            yield return null;
        }

        while (camera.orthographicSize > 0)
        {
            camera.orthographicSize -= Time.deltaTime * time;
            camera.transform.position = Vector3.Lerp(startPosition, targetPosition, 1 - camera.orthographicSize / time);
            if (camera.orthographicSize <= 0)
            {
                camera.orthographicSize = 0;
                GameManager.Instance.RestaurantState();
                yield break;
            }
            yield return null;
        }
    }
}

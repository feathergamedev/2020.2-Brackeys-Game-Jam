using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    InsertAction = 0,
    Walk,
    OpenTreasure,
    Killed,
    StageComplete,
    WrongAnswer,
    ScorpionAttack,
    ClickButton,
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField]
    private GameObject m_audioSourcePrefab;

    [SerializeField]
    private List<AudioClip> m_allSound;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound(SoundType type)
    {
        StartCoroutine(SoundPerform(type));
    }

    IEnumerator SoundPerform(SoundType type)
    {
        var newSound = Instantiate(m_audioSourcePrefab, Vector3.zero, Quaternion.identity, transform);

        var source = newSound.GetComponent<AudioSource>();

        source.clip = m_allSound[(int)type];

        source.Play();

        while (source.isPlaying)
            yield return null;

        Destroy(source.gameObject);
    }
}

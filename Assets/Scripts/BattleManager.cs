using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;


public enum BattleState
{
    Player,
    Enemys,
    noFight,
}

public enum facing
{
    up,
    down,
    left,
    right,
}

public enum result
{
    win,
    lose,
    neutral,
}

public class BattleManager : MonoBehaviour
{
    class BattleParticipant
    {
        public string nameChar;
        public int health;
        public int maxHealth;
        public int mAttack;
        public int defense;
        public int evade;
        public int speed;
        public List<skills> skillset;

        public BattleParticipant(BattleStats data)
        {
            nameChar = data.nameChar;
            health = data.maxHealth;
            maxHealth = data.maxHealth;
            defense = data.defense;
            mAttack = 1;
            evade = data.evade;
            speed = data.speed;
            skillset = new List<skills>();
            foreach (skills skill in data.Skills)
            {
                skillset.Add(new skills(skill));
            }
            skillset = skillset.OrderByDescending(x => x.priority).ToList();
        }
    }

    public BattleState battleState;
    public BattleStats playerStats;

    BattleParticipant player;
    [SerializeField] private List<BattleParticipant> enemies;
    [SerializeField] private GameObject BattleUI;
    [SerializeField] private GameObject skillPanel;
    [SerializeField] private List<GameObject> skillObj;
    [SerializeField] private List<GameObject> enemyPosition;
    [SerializeField] private List<GameObject> highlightTarget;
    [SerializeField] private GameObject playerPosition;
    [SerializeField] private GameObject healthPlayerUI;
    [SerializeField] private TextMeshProUGUI healthTextPlayer;
    [SerializeField] private Slider healthPlayerSlider;

    [SerializeField] private facing face;
    private result resultBattle;
    GameObject textBattle;
    GameObject buttonBattle;

    private Vector2 currentPlayerPosition;
    private int selectedTargetIndex = 0;
    private int selectedSkillIndex = 0;
    private bool changeCheck = false;
    private bool canClickSkill = true;
    private bool chooseTarget = false;
    private bool enemyTurnInProgress = false;
    public bool isBattleActive = false;
    private Coroutine skillCoroutine;
    private Coroutine deadCoroutine;
    private Coroutine attackMissCoroutine;
    private static BattleManager instance;

    public static BattleManager GetInstance()
    {
        return instance;
    }
    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("Multiple instances of BattleManager found! Destroying duplicate.");

            return;
        }
        instance = this;
        textBattle = BattleUI.transform.Find("BottomPanel(Text)").gameObject;
        buttonBattle = BattleUI.transform.Find("BottomPanel(PlayerButton)").gameObject;
        skillCoroutine = null;
    }

    void Update()
    {
        if (chooseTarget)
        {
            if (InputManager.GetInstance().GetMoveDirection() != Vector2.zero && !changeCheck)
            {
                changeCheck = true;
                if(InputManager.GetInstance().GetMoveDirection().x > 0)
                {
                    StartCoroutine(changeSelect(1));
                }
                else if (InputManager.GetInstance().GetMoveDirection().x < 0)
                {
                    StartCoroutine(changeSelect(-1));
                }
                return;
            }
            if(InputManager.GetInstance().GetSelectPressed() && skillCoroutine == null)
            {
                chooseTarget = false;
                skillPanel.SetActive(false);
                //buttonBattle.SetActive(true);
                PlayerTurn(selectedSkillIndex, selectedTargetIndex);
                highlightTarget[selectedTargetIndex].SetActive(false);
                canClickSkill = true;
            }
            return;
        }
        if (battleState == BattleState.noFight)
        {
            return;
        }
        if(battleState == BattleState.Enemys && !enemyTurnInProgress)
        {
            //Debug.Log("debug enemy turn");
            enemyTurnInProgress = true;
            StartCoroutine(enemyTurn());
        }
    }

    public void StartBattle(BattleStats[] enemyStats, List<GameObject> enemyPositions, GameObject playerPos)
    {
        isBattleActive = true;
        resultBattle = result.neutral;
        selectedTargetIndex = 0;
        skillPanel.SetActive(false);
        chooseTarget = false;
        canClickSkill = true;
        changeCheck = false;
        enemyTurnInProgress = false;
        enemies = new List<BattleParticipant>();
        enemyPosition = new List<GameObject>();
        highlightTarget = new List<GameObject>();
        switch (face)
        {
            case facing.up:
                PlayerOverworld.GetInstance().FaceUp();
                break;
            case facing.down:
                PlayerOverworld.GetInstance().FaceDown();
                break;
            case facing.right:
                PlayerOverworld.GetInstance().FaceRight();
                break;
            case facing:
                PlayerOverworld.GetInstance().FaceLeft();
                break;
            default:

        }
        player = new BattleParticipant(playerStats);
        healthPlayerSlider.maxValue = player.maxHealth;
        healthPlayerSlider.value = player.health;
        healthTextPlayer.text = $"{player.health}/{player.maxHealth}";
        StartCoroutine(transitionBeginBattle(playerPos, enemyStats, enemyPositions));
    }

    private IEnumerator transitionBeginBattle(GameObject playerPos, BattleStats[] enemyStats, List<GameObject> enemyPositions)
    {
        DialogueManager.GetInstance().fadeInScene.Play();
        yield return new WaitForSeconds(0.2f);
        currentPlayerPosition = new Vector2(PlayerOverworld.GetInstance().transform.position.x, PlayerOverworld.GetInstance().transform.position.y);
        PlayerOverworld.GetInstance().transform.position = playerPos.transform.position;
        yield return new WaitForSeconds(0.8f);
        DialogueManager.GetInstance().fadeOutScene.Play();
        yield return new WaitUntil(() => DialogueManager.GetInstance().fadeOutScene.state != UnityEngine.Playables.PlayState.Playing);
        battleState = BattleState.Player;
        BattleUI.SetActive(true);
        buttonBattle.SetActive(false);
        textBattle.SetActive(true);
        int index = 0;
        foreach (BattleStats enemyStat in enemyStats)
        {
            BattleParticipant enemy = new BattleParticipant(enemyStat);
            GameObject enemyGameObject = Instantiate(enemyStat.prefab, enemyPositions[index].transform.position, Quaternion.identity);
            Transform canvas = enemyGameObject.transform.Find("Canvas");
            canvas.transform.localPosition = Vector3.zero;
            canvas.TransformVector(new Vector3(3, 3, 0));
            enemyPosition.Add(enemyGameObject);
            enemyPosition[index].GetComponent<infoEnemy>().enemyHp.maxValue = enemy.maxHealth;
            enemyPosition[index].GetComponent<infoEnemy>().enemyHp.value = enemy.health;
            enemyPosition[index].GetComponent<infoEnemy>().enemyName.text = enemy.nameChar;
            enemyPosition[index].GetComponent<infoEnemy>().enemyHPpoint.text = $"{enemy.health}/{enemy.maxHealth}";
            highlightTarget.Add(enemyPositions[index].transform.Find("select").gameObject);
            enemies.Add(enemy);
            index++;
        }
        textBattle.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "";
        string text = "Enemy Approaching...";
        foreach(char c in text)
        {
            textBattle.GetComponentInChildren<TMPro.TextMeshProUGUI>().text += c;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.2f);
        textBattle.SetActive(false);
        buttonBattle.SetActive(true);
    }
    public void EndBattle()
    {
        StartCoroutine(resultShow());
        isBattleActive = false;
        battleState = BattleState.noFight;
    }

    private IEnumerator resultShow()
    {
        yield return new WaitUntil(() => skillCoroutine == null && deadCoroutine == null);
        textBattle.SetActive(true);
        buttonBattle.SetActive(false);
        if (resultBattle == result.win)
        {
            textBattle.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "";
            string text = "You Win!";
            foreach (char c in text)
            {
                textBattle.GetComponentInChildren<TMPro.TextMeshProUGUI>().text += c;
                yield return new WaitForSeconds(0.05f);
            }

        }
        else if (resultBattle == result.lose)
        {
            textBattle.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "";
            string text = "You Lose!";
            PlayerOverworld.GetInstance().animator.Play("Dead");
            foreach (char c in text)
            {
                textBattle.GetComponentInChildren<TMPro.TextMeshProUGUI>().text += c;
                yield return new WaitForSeconds(0.05f);
            }
        }
        yield return new WaitForSeconds(0.5f);
        BattleUI.SetActive(false);
        DialogueManager.GetInstance().fadeInScene.Play();
        yield return new WaitForSeconds(0.3f);
        foreach (GameObject enemyGO in enemyPosition)
        {
            Destroy(enemyGO);
        }
        PlayerOverworld.GetInstance().transform.position = currentPlayerPosition;
        yield return new WaitForSeconds(0.8f);
        DialogueManager.GetInstance().fadeOutScene.Play();
        PlayerOverworld.GetInstance().isFreeze = false;
        currentPlayerPosition = Vector2.zero;
        PlayerOverworld.GetInstance().animator.Play("Idle");
    }

    private IEnumerator enemyTurn()
    {
        //Debug.Log("Enemy turn!");
        bool checkEnemyAlive = false;
        if(resultBattle != result.neutral)
        {
            yield break;
        }
        foreach (BattleParticipant enemy in enemies)
        {
            if (resultBattle != result.neutral)
            {
                yield break;
            }
            if (enemy.health <= 0)
            {
                continue;
            }
            checkEnemyAlive = true;
            if (enemy.speed == 0)
            {
                enemy.speed = 1;
            }
            else
            {
                bool useSkill = false;
                foreach (skills skill in enemy.skillset)
                {
                    if (skill.cooldownRemaining <= 0 && !useSkill)
                    {
                        switch (skill.type)
                        {
                            case SkillType.Attack:
                                {
                                    //Debug.Log("enemy used attack");
                                    useSkill = true;
                                    skillCoroutine = StartCoroutine(useSkillUI(skill.name, enemy.nameChar));
                                    PlayerGetDamage(skill.damage, skill.accuracy);
                                    break;
                                }
                            case SkillType.Heal:
                                {
                                    foreach (BattleParticipant ally in enemies)
                                    {
                                        if (ally.health < ally.maxHealth && ally.health < (ally.maxHealth*70/100))
                                        {
                                            useSkill = true;
                                            skillCoroutine = StartCoroutine(useSkillUI(skill.name, enemy.nameChar));
                                            ally.health += skill.heal;
                                            if (ally.health > ally.maxHealth)
                                            {
                                                ally.health = ally.maxHealth;
                                            }
                                            break;
                                        }
                                    }
                                    break;
                                }
                            case SkillType.Evade:
                                {
                                    // Handle evade skill
                                    break;
                                }
                            default: break;
                        }
                        if (useSkill) {
                            skill.cooldownRemaining = skill.cooldown;
                        }
                    }
                    skill.cooldownRemaining -= 1;
                }
                enemy.speed += 1;
                yield return new WaitUntil(() => skillCoroutine == null);
            }
        }
        if (resultBattle != result.neutral)
        {
            yield break;
        }
        if (!checkEnemyAlive)
        {
            resultBattle = result.win;
            yield return new WaitForSeconds(0.2f);
            EndBattle();
        }
        else
        {
            StartCoroutine(changeState());
        }
        yield return null;
    }

    private IEnumerator changeState()
    {
        yield return new WaitUntil(() => skillCoroutine == null && deadCoroutine == null && attackMissCoroutine == null);
        enemies = enemies.OrderByDescending(x => x.speed).ToList();
        yield return new WaitForSeconds(0.5f);
        if (battleState == BattleState.Player)
        {
            textBattle.SetActive(true);
            buttonBattle.SetActive(false);
            battleState = BattleState.Enemys;
            enemyTurnInProgress = false;
        }
        else if (battleState == BattleState.Enemys)
        {
            //Debug.Log("test change");
            textBattle.SetActive(false);
            buttonBattle.SetActive(true);
            battleState = BattleState.Player;
        }
    }
    private IEnumerator useSkillUI(string skillName, string ownerName)
    {
        buttonBattle.SetActive(false);
        textBattle.SetActive(true);
        textBattle.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "";
        string text = $"{ownerName} used {skillName}!";
        foreach (char c in text)
        {
            textBattle.GetComponentInChildren<TMPro.TextMeshProUGUI>().text += c;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.5f);
        skillCoroutine = null;
    }
    
    public void PlayerTurn(int moveIndex, int targetIndex)
    {
        if (battleState != BattleState.Player || player.speed == -1)
        {
            Debug.Log("It's not the player's turn!");
            return;
        }
        BattleParticipant target = enemies[targetIndex];
        PlayerAttack(target, moveIndex, targetIndex);
    }

    public void showSkill()
    {
        buttonBattle.SetActive(false);
        textBattle.SetActive(true);
        textBattle.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Choose a skill!";
        skillPanel.SetActive(true);
        int index = 0;
        foreach (var skill in player.skillset)
        {
            skillObj[index].SetActive(true);
            skillObj[index].transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = skill.name;
            GameObject coolDownPanel = skillObj[index].transform.Find("CooldownPanel").gameObject;
            if (skill.cooldownRemaining <= 0)
            {
                coolDownPanel.SetActive(false);
            }
            else
            {
                coolDownPanel.SetActive(true);
                coolDownPanel.transform.Find("CooldownCount").GetComponent<TextMeshProUGUI>().text = skill.cooldownRemaining.ToString();
            }
            index++;
        }
        if(index < skillObj.Count)
        {
            for (int i = index; i < skillObj.Count; i++)
            {
                skillObj[i].SetActive(false);
            }
        }
    }

    public void afterSkill(int moveIndex)
    {
        selectedSkillIndex = moveIndex;
        if (player.skillset[moveIndex].cooldownRemaining > 0)
        {
            Debug.Log("Skill is on cooldown!");
            return;
        }
        if (!canClickSkill) return;
        canClickSkill = false;
        showTarget();
    }

    void PlayerAttack(BattleParticipant target, int moveIndex, int targetIndex)
    {
        enemyPosition[targetIndex].GetComponent<Animator>().SetBool("getSelect", false);
        skillCoroutine = StartCoroutine(useSkillUI(player.skillset[moveIndex].name, ((Ink.Runtime.StringValue)DialogueManager.GetInstance().dialogueVariables.variableDictionary["MCName"]).value));
        player.skillset[moveIndex].cooldownRemaining = player.skillset[moveIndex].cooldown;
        int checkAcc = Random.Range(target.evade, 100);
        int playerAcc = Random.Range(player.skillset[moveIndex].accuracy, 100);
        //Debug.Log($"Player accuracy: {playerAcc}, Target evade: {checkAcc}");
        if (playerAcc < checkAcc)
        {
            attackMissCoroutine = StartCoroutine(attackMiss());
            StartCoroutine(changeState());
            foreach (var skill in player.skillset)
            {
                if (skill.cooldownRemaining > 0)
                {
                    skill.cooldownRemaining--;
                }
            }
            return;
        }
        enemyPosition[targetIndex].GetComponent<Animator>().SetTrigger("GetHit");
        target.health -= player.skillset[moveIndex].damage;
        enemyPosition[targetIndex].GetComponent<infoEnemy>().enemyHp.value = target.health;
        enemyPosition[targetIndex].GetComponent<infoEnemy>().enemyHPpoint.text = $"{target.health}/{target.maxHealth}";
        //Debug.Log($"Target health after attack: {target.health}");
        if (target.health <= 0)
        {
            target.health = 0;
            enemyPosition[targetIndex].GetComponent<infoEnemy>().enemyHp.value = target.health;
            enemyPosition[targetIndex].GetComponent<infoEnemy>().enemyHPpoint.text = $"{target.health}/{target.maxHealth}";
            deadCoroutine = StartCoroutine(deadEnemy(targetIndex));
        }
        foreach (var skill in player.skillset)
        {
            if (skill.cooldownRemaining > 0)
            {
                skill.cooldownRemaining--;
            }
        }
        StartCoroutine(changeState());
    }

    private IEnumerator deadEnemy(int targetIndex)
    {
        yield return new WaitUntil(() => skillCoroutine == null);
        textBattle.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "";
        string text = $"{enemies[targetIndex].nameChar} defeated!";
        enemyPosition[targetIndex].GetComponent<Animator>().Play("Dead");
        foreach (char c in text)
        {
            textBattle.GetComponentInChildren<TMPro.TextMeshProUGUI>().text += c;
            yield return new WaitForSeconds(0.05f);
        }
        enemies.RemoveAt(targetIndex);
        GameObject enemyRemove = enemyPosition[targetIndex];
        GameObject highlightRemove = highlightTarget[targetIndex];
        highlightTarget.RemoveAt(targetIndex);
        enemyPosition.RemoveAt(targetIndex);
        Destroy(enemyRemove);
        //Destroy(highlightRemove);
        yield return new WaitForSeconds(0.2f);
        deadCoroutine = null;
    }

    public void PlayerGetDamage(int damage, int accuracy)
    {
        int checkAcc = Random.Range(player.evade, 100);
        int enemyAcc = Random.Range(accuracy, 100);
        if (enemyAcc < checkAcc && enemyAcc != checkAcc)
        {
            StartCoroutine(attackMiss());
            return;
        }
        //Debug.Log("Attack hit!");
        PlayerOverworld.GetInstance().animator.SetTrigger("GetHit");
        player.health -= damage;
        healthPlayerSlider.value = (float)player.health;
        healthTextPlayer.text = $"{player.health}/{player.maxHealth}";
        //Debug.Log($"Player health after attack: {player.health}");
        if (player.health <= 0)
        {
            player.health = 0;
            healthTextPlayer.text = $"{player.health}/{player.maxHealth}";
            //Debug.Log("Player defeated!");
            resultBattle = result.lose;
            EndBattle();
            return;
        }
    }

    private IEnumerator attackMiss()
    {
        yield return new WaitUntil(() => skillCoroutine == null && deadCoroutine == null);
        textBattle.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "";
        string text = "Attack missed!";
        foreach (char c in text)
        {
            textBattle.GetComponentInChildren<TMPro.TextMeshProUGUI>().text += c;
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void showTarget()
    {
        //Debug.Log("Choose a target!");
        foreach (var target in highlightTarget)
        {
            target.SetActive(false);
        }
        selectedTargetIndex = 0;
        chooseTarget = true;
        canClickSkill = false;
        enemyPosition[selectedTargetIndex].GetComponent<Animator>().SetBool("getSelect", true);
    }


    private IEnumerator changeSelect(int index)
    {
        enemyPosition[selectedTargetIndex].GetComponent<Animator>().SetBool("getSelect", false);
        if(index + selectedTargetIndex < 0)
        {
            selectedTargetIndex = enemies.Count - 1;
        }
        else if (index + selectedTargetIndex >= enemies.Count)
        {
            selectedTargetIndex = 0;
        }
        else
        {
            selectedTargetIndex += index;
        }
        enemyPosition[selectedTargetIndex].GetComponent<Animator>().SetBool("getSelect", true);
        yield return new WaitForSeconds(0.2f);
        changeCheck = false;
    }

}
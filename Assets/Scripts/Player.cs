using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Flip Settings")]
    [SerializeField] private float flipDuration = 0.2f;

    [Header("Components")]
    [SerializeField] private Animator animator;

    private float horizontalInput;
    private Vector3 movement;
    private bool isFacingRight = true;
    private Tween flipTween;

    private void Start()
    {
        // Pega o Animator se não foi atribuído
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Calcula o movimento
        movement = new Vector3(horizontalInput * moveSpeed * Time.deltaTime, 0, 0);

        // Move o sprite
        transform.position += movement;

        // Vira o sprite baseado na direção usando DOTween
        if (horizontalInput > 0 && !isFacingRight)
            Flip(true);
        else if (horizontalInput < 0 && isFacingRight)
            Flip(false);

        // Atualiza o parâmetro speed do Animator
        animator.SetFloat("speed", Mathf.Abs(horizontalInput));
    }

    private void Flip(bool facingRight)
    {
        isFacingRight = facingRight;

        // Cancela o flip anterior se ainda estiver executando
        flipTween?.Kill();

        // Anima o flip com DOTween
        float targetScaleX = facingRight ? 1 : -1;
        flipTween = transform.DOScaleX(targetScaleX, flipDuration).SetEase(Ease.OutBack);
    }

    // Método chamado pelo Player Input component
    public void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        horizontalInput = input.x;
    }

    private void OnDestroy()
    {
        // Limpa o tween ao destruir o objeto
        flipTween?.Kill();
    }

}
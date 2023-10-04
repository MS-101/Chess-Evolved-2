using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PieceInfoController : MonoBehaviour
{
    [SerializeField] private Button closeBtn;
    [SerializeField] private GameObject board;
    [SerializeField] private EngineController engineController;

    private GameObject piecePrefab, movementPrefab;
    private PieceObject pieceObject;
    private List<MovementObject> movements = new();

    private void Awake()
    {
        piecePrefab = (GameObject)Resources.Load("Prefabs/Piece");
        movementPrefab = (GameObject)Resources.Load("Prefabs/Movement");

        CreatePiece();
    }

    private void Start()
    {
        closeBtn.onClick.AddListener(OnCloseButtonClicked);
    }

    private Piece myPiece;
    public Piece MyPiece
    {
        get { return myPiece; }
        set
        {
            myPiece = value;

            UpdateDisplayedPiece();
        }
    }

    private void CreatePiece()
    {
        pieceObject = Instantiate(piecePrefab, board.transform).GetComponent<PieceObject>();
        pieceObject.Piece = new(Chess.PieceType.Pawn, Chess.Color.White, Chess.Essence.Classic);

        RectTransform parentRect = board.gameObject.GetComponent<RectTransform>();

        float baseX = parentRect.position.x - parentRect.rect.width / 2;
        float baseY = parentRect.position.y - parentRect.rect.height / 2;
        float width = parentRect.rect.width / 7;
        float height = width;

        RectTransform rect = pieceObject.gameObject.GetComponent<RectTransform>();
        rect.position = new(baseX + 3 * width, baseY + 3 * height);
        rect.sizeDelta = new(width, height);
    }

    private void UpdateDisplayedPiece()
    {
        pieceObject.Piece = new(myPiece.type, myPiece.color, myPiece.essence);
        while (movements.Count > 0)
        {
            Destroy(movements.Last().gameObject);
            movements.RemoveAt(movements.Count - 1);
        }

        RectTransform parentRect = board.gameObject.GetComponent<RectTransform>();

        float baseX = parentRect.position.x - parentRect.rect.width / 2;
        float baseY = parentRect.position.y - parentRect.rect.height / 2;
        float width = parentRect.rect.width / 7;
        float height = parentRect.rect.height / 7;

        List<Mobility> mobilities = engineController.mobilities[myPiece.type][myPiece.essence];
        foreach (Mobility mobility in mobilities)
        {
            int x = 3 + mobility.start_x;
            int y = 3 + mobility.start_y;
            int moveCounter = 0;

            while (x >= 0 && x <= 6 && y >= 0 && y <= 6 && (mobility.limit == 0 || moveCounter < mobility.limit))
            {
                MovementObject movementObject = Instantiate(movementPrefab, board.transform).GetComponent<MovementObject>();
                movementObject.Movement = new(pieceObject.Piece, mobility.type, x, y);

                RectTransform rect = movementObject.gameObject.GetComponent<RectTransform>();
                rect.position = new(baseX + x * width, baseY + y * height);
                rect.sizeDelta = new(width, height);

                movements.Add(movementObject);

                x += mobility.direction_x;
                y += mobility.direction_y;
                moveCounter++;
            }
        }
    }

    private void OnCloseButtonClicked()
    {
        gameObject.SetActive(false);
    }
}

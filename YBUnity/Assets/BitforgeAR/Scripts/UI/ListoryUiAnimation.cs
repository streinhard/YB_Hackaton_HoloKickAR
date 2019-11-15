using System.Collections;
using System.Diagnostics.SymbolStore;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class ListoryUiAnimation : MonoBehaviour
    {
        public GameObject blackCurtain;
        
        public RectTransform blueBgAnchor;
        public RectTransform blueFgAnchor;
        public RectTransform blueCastleAnchor;
    
        public RectTransform redBgAnchor;
        public RectTransform redFgAnchor;
        public RectTransform redTreesAnchor;
        public RectTransform redSingleTreeAnchor;
    
        public RectTransform roadAnchor;
        public RectTransform markerAnchor;
        public RectTransform sponsorCard;

        private float _screenWidth;
        private float _screenHeight;
        
        private Vector2 _startValueBlueBgAnchor;
        private Vector2 _startValueBlueFgAnchor;
        private Vector2 _startValueRedBgAnchor;
        private Vector2 _startValueRedFgAnchor;
        
        private Vector2 _startValueMarkerAnchor;
        private Vector2 _startValueBlueCastleAnchor;

        private Vector2 _startValueSponsorCardAnchor;
              
        private void Awake()
        {
            _screenWidth = Screen.width;
            _screenHeight = Screen.height;

            _startValueBlueBgAnchor = blueBgAnchor.anchoredPosition;
            _startValueBlueFgAnchor = blueFgAnchor.anchoredPosition;
            
            _startValueRedBgAnchor = redBgAnchor.anchoredPosition;
            _startValueRedFgAnchor = redFgAnchor.anchoredPosition;

            _startValueMarkerAnchor = markerAnchor.anchoredPosition;
            _startValueBlueCastleAnchor = blueCastleAnchor.anchoredPosition;
            _startValueSponsorCardAnchor = sponsorCard.anchoredPosition;

        }
        public Tween CreateIntroAnimation()
        {
            var introAnimation = DOTween.Sequence();

            var mountains = MountainsSequence();
            var treesAndCastle = TreesAndCastleSequence();
            var roads = RoadSequence();
            var marker = MarkerSequence();
            var sponsor = SponsorSequence();


            introAnimation.Insert(0f, sponsor);
            introAnimation.Insert(0.5f, mountains); // duration 0.675 
            introAnimation.Insert( 1f, treesAndCastle); // duration 0.75
            introAnimation.Insert(1.5f, roads); // duration .25
            introAnimation.Insert( 1.5f, marker); // .75
            //introAnimation.PrependInterval(1.5f);
            //introAnimation.timeScale = .1f;
            return introAnimation;
        }
        public void CreateSpinnerAnimation()
        {
            var markerSpinnerSequence = DOTween.Sequence();

            var tween = markerAnchor.DORotate(new Vector3(0, (360 * 2), 0), 1.25f, RotateMode.FastBeyond360).SetEase(Ease.OutCubic);
            markerSpinnerSequence.PrependInterval(2);
            markerSpinnerSequence.Insert(0f, tween).SetLoops(-1);
        }
        
        private Sequence MountainsSequence()
        {   
            var mountainsSequence = DOTween.Sequence();
        
            // Mountains Start Values
            blueBgAnchor.anchoredPosition = new Vector2(-(_screenWidth / 6f) * 5, -_screenWidth/2f); // 5/6 der Screen Breite hinaus stellen
            blueFgAnchor.anchoredPosition = new Vector2(-(_screenWidth / 2f), -_screenWidth/3f); // 1/2 der Screen Breite Raustellen
        
            redBgAnchor.anchoredPosition = new Vector2((_screenWidth / 10f) * 7, -_screenWidth/2f); // 7/10 der Screenbreite Raus stellen
            redFgAnchor.anchoredPosition = new Vector2((_screenWidth / 5f) * 3, -_screenWidth/3f); // 3/5 der Screenbreite Raus

            // Animations
            mountainsSequence.Insert(0.0875f, blueBgAnchor.DOAnchorPosX(_startValueBlueBgAnchor.x, 0.5f).SetEase(Ease.InOutCubic));
            mountainsSequence.Insert(0.0875f, blueBgAnchor.DOAnchorPosY(_startValueBlueBgAnchor.y, 0.5f).SetEase(Ease.InOutCubic));
            
            mountainsSequence.Insert(.25f, blueFgAnchor.DOAnchorPosX(_startValueBlueFgAnchor.x, 0.5f).SetEase(Ease.InCubic));
            mountainsSequence.Insert(.25f, blueFgAnchor.DOAnchorPosY(_startValueBlueFgAnchor.y, 0.5f).SetEase(Ease.InOutCubic));
            
            mountainsSequence.Insert(0f, redBgAnchor.DOAnchorPosX(_startValueRedBgAnchor.x, 0.5f).SetEase(Ease.InOutCubic));
            mountainsSequence.Insert(0f, redBgAnchor.DOAnchorPosY(_startValueRedBgAnchor.y, 0.5f).SetEase(Ease.InOutCubic));
            
            mountainsSequence.Insert(.175f, redFgAnchor.DOAnchorPosX(_startValueRedFgAnchor.x, 0.5f).SetEase(Ease.InCubic));
            mountainsSequence.Insert(.175f, redFgAnchor.DOAnchorPosY(_startValueRedFgAnchor.y, 0.5f).SetEase(Ease.InOutCubic));

            return mountainsSequence;
        }
        private Sequence TreesAndCastleSequence()
        {
            var treesAndCastleSequence = DOTween.Sequence();
        
            // Trees / Castle Start Values
            blueCastleAnchor.anchoredPosition = new Vector2(0, -(_screenWidth *0.1f));
            blueCastleAnchor.localScale = Vector3.zero;
        
            treesAndCastleSequence.Insert(0f, blueCastleAnchor.DOScale(1, 0.5f).SetEase(Ease.InCubic));
            treesAndCastleSequence.Insert(0f, blueCastleAnchor.DOAnchorPosY(_startValueBlueCastleAnchor.y, .75f).SetEase(Ease.OutElastic));
            
            var otherTrees = redTreesAnchor.GetComponentsInChildren<RectTransform>();
            // this tree is below the road
            var singleTree = redSingleTreeAnchor.GetComponentsInChildren<RectTransform>();
            
            var trees = otherTrees.Concat(singleTree).ToArray();

            var offset = 0f;
        
            foreach (var tree in trees)
            {
                if (!(tree.parent == redTreesAnchor ^ tree.parent == redSingleTreeAnchor)) continue;
                offset += Random.value * 0.2f;
                tree.localScale = Vector3.zero;
                treesAndCastleSequence.Insert(0.0175f + offset, tree.DOScale(1, 0.75f).SetEase(Ease.OutElastic));
            }

            return treesAndCastleSequence;
        }
        private Sequence RoadSequence()
        {
            var roadSequence = DOTween.Sequence();
            var roadSprite = roadAnchor.GetComponentInChildren<Image>();

            roadSprite.fillAmount = 0;

            var roadTween = roadSprite.DOFillAmount(1f, 0.25f).SetEase(Ease.OutCirc);

            roadSequence.Insert(0f, roadTween);

            return roadSequence;
        }
        private Sequence MarkerSequence()
        {
            var markerSequence = DOTween.Sequence();
        
            // Start State
            markerAnchor.anchoredPosition = new Vector2(0f, -(_screenHeight / 2f));
            markerAnchor.localScale = Vector3.zero;

            markerSequence.Insert(0f, markerAnchor.DOScale(1f, 0.75f).SetEase(Ease.OutCubic));
            markerSequence.Insert(0f, markerAnchor.DOMoveY(-(_screenHeight / 16f), 0.5f).SetEase(Ease.Linear));
            markerSequence.Insert(0f, markerAnchor.DOMoveY(_startValueMarkerAnchor.y, 0.75f).SetEase(Ease.OutBack));
            markerSequence.Insert(0f, markerAnchor.DORotate(new Vector3(0f,(360 * 2),0f), 1.25f, RotateMode.FastBeyond360).SetEase(Ease.OutCubic));

            return markerSequence;
        }
        private Sequence SponsorSequence()
        {
            var sponsorSequence = DOTween.Sequence();
            
            // Start State
            sponsorCard.anchoredPosition = new Vector2(0f, (sponsorCard.rect.height/2) *-1);

            sponsorSequence.Insert(0f, sponsorCard.DOAnchorPos(_startValueSponsorCardAnchor, 0.5f).SetEase(Ease.OutCubic));

            return sponsorSequence;

        }
    }
}

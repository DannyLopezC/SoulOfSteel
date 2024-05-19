using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NUnit.Framework;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using NSubstitute.ReturnsExtensions;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

namespace SoulOfSteelTests {
    public class CardControllerTests {
        private ICardView _mockView;
        private IGameManager _mockGameManager;
        private IUIManager _mockUIManager;
        private IScrapPanel _mockScrapPanel;
        private PlayerView _mockPlayerView;
        private GameObject _gameObject;

        [SetUp]
        public void BeforeTest()
        {
            _mockView = Substitute.For<ICardView>();
            _mockGameManager = Substitute.For<IGameManager>();
            _mockUIManager = Substitute.For<IUIManager>();
            _mockScrapPanel = Substitute.For<IScrapPanel>();
            _mockPlayerView = Substitute.For<PlayerView>();

            _gameObject = new GameObject();
            _mockPlayerView._inAnimation = false;
            _mockGameManager.ScrapPanel.Returns(_mockScrapPanel);
            _mockGameManager.LocalPlayerInstance.Returns(_mockPlayerView);
            _mockView.GetGameObject().Returns(_gameObject);
        }

        private CardController CreateSystem()
        {
            return Substitute.For<CardController>(_mockView, _mockGameManager, _mockUIManager);
        }

        #region DismissCard

        // [Test]
        // public void DismissCard_CardIsDismissedSuccessfully()
        // {
        //     // Arrange.
        //     var cardController = CreateSystem();
        //     var scrapPanelGameObject = new GameObject();
        //     var scrapPanelTransform = scrapPanelGameObject.transform;
        //     var scrapPanelChildGameObject = new GameObject();
        //     var scrapPanelChildTransform = scrapPanelChildGameObject.transform;
        //
        //     _mockScrapPanel.transform.Returns(scrapPanelTransform);
        //     scrapPanelTransform.TransformPoint(Arg.Any<Vector3>()).Returns(Vector3.zero);
        //     scrapPanelTransform.GetChild(0).Returns(scrapPanelChildTransform);
        //     scrapPanelChildTransform.localScale = Vector3.one;
        //
        //     // Mocking DOTween's DOMove and OnComplete
        //     Tween tween = Substitute.For<Tween>();
        //     _transform.DOMove(Vector3.zero, 0.5f).Returns(tween);
        //     tween.OnComplete(Arg.Do<TweenCallback>(callback => callback.Invoke())).Returns(tween);
        //
        //     // Act.
        //     cardController.DismissCard();
        //
        //     // Assert.
        //     _transform.Received(1).DOMove(Vector3.zero, 0.5f);
        //     Assert.AreEqual(Vector3.one, _transform.localScale);
        //     Assert.AreSame(scrapPanelTransform, _transform.parent);
        //     Assert.AreEqual(2, _transform.GetSiblingIndex());
        //     _mockView.Received(1).SetDismissTextSizes();
        //     Assert.IsTrue(_playerView._inAnimation);
        // }
        //
        // [Test]
        // public void DismissCard_ScrapPanelInteraction()
        // {
        //     // Arrange.
        //     var cardController = CreateSystem();
        //     var scrapPanelGameObject = new GameObject();
        //     var scrapPanelTransform = scrapPanelGameObject.transform;
        //
        //     _mockScrapPanel.transform.Returns(scrapPanelTransform);
        //     scrapPanelTransform.TransformPoint(Arg.Any<Vector3>()).Returns(Vector3.zero);
        //
        //     // Act.
        //     cardController.DismissCard();
        //
        //     // Assert.
        //     _mockScrapPanel.Received(1).SendToBackup();
        // }

        [Test]
        public void DismissCard_AnimationSequence()
        {
            // Arrange.
            var cardController = CreateSystem();
            var scrapPanelTransform = new GameObject().transform;

            _mockScrapPanel.GetTransform().Returns(scrapPanelTransform);
            scrapPanelTransform.TransformPoint(Arg.Any<Vector3>()).Returns(Vector3.zero);

            // Mocking DOTween's DOMove and OnComplete
            Tween tween = Substitute.For<Tween>();
            _gameObject.transform.DOMove(Vector3.zero, 0.5f).Returns(tween);
            tween.OnComplete(Arg.Do<TweenCallback>(callback => callback.Invoke())).Returns(tween);

            // Act.
            cardController.DismissCard();

            // Assert.
            _gameObject.transform.Received(1).DOMove(Vector3.zero, 0.5f);
            Assert.IsTrue(_mockPlayerView._inAnimation);
        }

        // [Test]
        // public void DismissCard_ErrorHandling_NullScrapPanel()
        // {
        //     // Arrange.
        //     var cardController = CreateSystem();
        //     _mockGameManager.ScrapPanel.Returns((ScrapPanel)null);
        //
        //     // Act & Assert.
        //     Assert.DoesNotThrow(() => cardController.DismissCard());
        // }
        //
        // [Test]
        // public void DismissCard_ErrorHandling_InAnimationAlreadyTrue()
        // {
        //     // Arrange.
        //     var cardController = CreateSystem();
        //     _playerView._inAnimation = true;
        //
        //     // Act.
        //     cardController.DismissCard();
        //
        //     // Assert.
        //     _transform.DidNotReceive().DOMove(Arg.Any<Vector3>(), Arg.Any<float>());
        // }

        #endregion
    }
}
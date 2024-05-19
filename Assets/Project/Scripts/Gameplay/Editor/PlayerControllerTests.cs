using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using NSubstitute.ReturnsExtensions;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

namespace SoulOfSteelTests {
    public class PlayerControllerTests {
        private IPlayerView _mockView;
        private IGameManager _mockGameManager;
        private IEffectManager _mockEffectManager;
        private IUIManager _mockUIManager;
        private IArmCardView _mockCardView;

        [SetUp]
        public void BeforeTest()
        {
            _mockView = Substitute.For<IPlayerView>();
            _mockGameManager = Substitute.For<IGameManager>();
            _mockEffectManager = Substitute.For<IEffectManager>();
            _mockUIManager = Substitute.For<IUIManager>();
            _mockCardView = Substitute.For<IArmCardView>();
        }

        private PlayerController CreateSystem()
        {
            return new PlayerController(_mockView, _mockGameManager, _mockEffectManager, _mockUIManager);
        }

        #region DrawCards

        [Test]
        public void DrawCards_WhenFullDrawFalse_InitAddCards(
            [Values(0, 1, 2, 3, 4, 5, 6, 7, 8, 9)] int amount)
        {
            // Arrange.
            _mockGameManager.HandPanel.Returns(Substitute.For<IHandPanel>());

            // Act.
            PlayerController systemUnderTest = CreateSystem();
            systemUnderTest.DrawCards(amount, false);

            // Assert.
            _mockGameManager.HandPanel.DidNotReceive().ResetAnimationReferenceParent();
            _mockView.Received(1).InitAddCards(amount);
        }

        [Test]
        public void DrawCards_WhenFullDrawTrue_InitAddCards(
            [NUnit.Framework.Range(0, 9)] int amount)
        {
            // Arrange.
            _mockGameManager.HandPanel.Returns(Substitute.For<IHandPanel>());

            // Act.
            PlayerController systemUnderTest = CreateSystem();
            systemUnderTest.DrawCards(amount, true);

            // Assert.
            _mockGameManager.HandPanel.Received(1).ResetAnimationReferenceParent();
            _mockView.Received(1).CleanHandsPanel();
            _mockView.Received(1).InitAddCards(amount);
        }

        #endregion

        #region TryPayingForCard

        [Test]
        public void TryPayingForCard_EnoughScrapPoints_DecreaseScrapPoints_AndReturnsTrue()
        {
            // Arrange.
            PlayerController systemUnderTest = CreateSystem();
            int initialScrapPoint = systemUnderTest.Debug_GetScrapPoints();
            const int cardCost = 5;

            // Act.
            bool result = systemUnderTest.TryPayingForCard(cardCost);

            // Assert.
            Assert.AreEqual(systemUnderTest.Debug_GetScrapPoints(), initialScrapPoint - cardCost);
            Assert.IsTrue(result);
        }

        [Test]
        public void TryPayingForCard_EnoughScrapPoints_ScrapPointsDoesNotChange_AndReturnsFalse()
        {
            // Arrange.
            PlayerController systemUnderTest = CreateSystem();
            int initialScrapPoint = systemUnderTest.Debug_GetScrapPoints();
            int cardCost = initialScrapPoint + 5;

            // Act.
            bool result = systemUnderTest.TryPayingForCard(cardCost);

            // Assert.
            Assert.AreEqual(systemUnderTest.Debug_GetScrapPoints(), initialScrapPoint);
            Assert.IsFalse(result);
        }

        #endregion

        #region EquipCard

        [Test]
        public void EquipCard_WhenCardInfoStructIsNull_LogsCardNotFoundError()
        {
            // Arrange.
            const int index = 1;
            _mockGameManager.GetCardFromDataBaseByIndex(index).Returns((CardInfoSerialized.CardInfoStruct)null);
            PlayerController systemUnderTest = CreateSystem();

            // Act.
            systemUnderTest.EquipCard(index);

            // Assert.
            LogAssert.Expect(LogType.Error, "CARD NOT FOUND");
        }

        [Test]
        public void EquipCard_WhenCardTypeIsArm_SetsArmCard()
        {
            // Arrange.
            const int index = 1;
            var cardInfoStruct = new CardInfoSerialized.CardInfoStruct {
                Id = 1,
                CardName = "Test Arm Card",
                Description = "Test Description",
                Cost = 10,
                Recovery = 2,
                Damage = 5,
                AttackTypeEnum = AttackType.StraightLine,
                AttackDistance = 2,
                AttackArea = 1,
                ImageSource = null,
                TypeEnum = CardType.Arm
            };

            _mockGameManager.GetCardFromDataBaseByIndex(index).Returns(cardInfoStruct);
            _mockView.AddCardToPanel(CardType.Arm, false).Returns(_mockCardView);

            // Act.
            PlayerController systemUnderTest = CreateSystem();
            systemUnderTest.EquipCard(index);

            // Assert.
            _mockCardView.Received(1).InitCard(
                cardInfoStruct.Id,
                cardInfoStruct.CardName,
                cardInfoStruct.Description,
                cardInfoStruct.Cost,
                cardInfoStruct.Recovery,
                cardInfoStruct.Damage,
                cardInfoStruct.AttackTypeEnum,
                cardInfoStruct.AttackDistance,
                cardInfoStruct.AttackArea,
                cardInfoStruct.ImageSource,
                cardInfoStruct.TypeEnum
            );

            Assert.AreSame(_mockCardView, systemUnderTest.GetArmCard()); // Check if _arm is set correctly.
        }

        [Test]
        public void EquipCard_WhenCardTypeIsWeapon_SetsArmCard()
        {
            // Arrange.
            const int index = 1;
            var cardInfoStruct = new CardInfoSerialized.CardInfoStruct { TypeEnum = CardType.Weapon };
            _mockGameManager.GetCardFromDataBaseByIndex(index).Returns(cardInfoStruct);
            PlayerController systemUnderTest = CreateSystem();

            // Act.
            systemUnderTest.EquipCard(index);

            // Assert.
            // Add your assertions to verify that SetArmCard was called with correct parameters.
        }

        [Test]
        public void EquipCard_WhenCardTypeIsLegs_SetsLegsCard()
        {
            // Arrange.
            const int index = 1;
            var cardInfoStruct = new CardInfoSerialized.CardInfoStruct { TypeEnum = CardType.Legs };
            _mockGameManager.GetCardFromDataBaseByIndex(index).Returns(cardInfoStruct);
            PlayerController systemUnderTest = CreateSystem();

            // Act.
            systemUnderTest.EquipCard(index);

            // Assert.
            // Add your assertions to verify that SetLegsCard was called with correct parameters.
        }

        [Test]
        public void EquipCard_WhenCardTypeIsArmor_SetsArmorCard()
        {
            // Arrange.
            const int index = 1;
            var cardInfoStruct = new CardInfoSerialized.CardInfoStruct { TypeEnum = CardType.Armor };
            _mockGameManager.GetCardFromDataBaseByIndex(index).Returns(cardInfoStruct);
            PlayerController systemUnderTest = CreateSystem();

            // Act.
            systemUnderTest.EquipCard(index);

            // Assert.
            // Add your assertions to verify that SetArmorCard was called with correct parameters.
        }

        [Test]
        public void EquipCard_WhenCardTypeIsChest_SetsArmorCard()
        {
            // Arrange.
            const int index = 1;
            var cardInfoStruct = new CardInfoSerialized.CardInfoStruct { TypeEnum = CardType.Chest };
            _mockGameManager.GetCardFromDataBaseByIndex(index).Returns(cardInfoStruct);
            PlayerController systemUnderTest = CreateSystem();

            // Act.
            systemUnderTest.EquipCard(index);

            // Assert.
            // Add your assertions to verify that SetArmorCard was called with correct parameters.
        }

        #endregion

        #region ShuffleDeck

        [Test]
        public void ShuffleDeck_WithoutShuffle_RemainsUnchanged()
        {
            // Arrange
            var mockPlayerCardsInfo = Substitute.For<PlayerCardsInfo>();
            var cardInfo1 = new CardInfoSerialized.CardInfoStruct { Id = 1, CardName = "Card1" };
            var cardInfo2 = new CardInfoSerialized.CardInfoStruct { Id = 2, CardName = "Card2" };
            var cardInfo3 = new CardInfoSerialized.CardInfoStruct { Id = 3, CardName = "Card3" };
            mockPlayerCardsInfo.playerCards.Returns(new List<CardInfoSerialized.CardInfoStruct>() {
                cardInfo1,
                cardInfo2,
                cardInfo3
            });
            _mockView.GetDeckInfo().Returns(mockPlayerCardsInfo);

            // Act
            PlayerController systemUnderTest = CreateSystem();
            systemUnderTest.ShuffleDeck(false, false);

            // Assert
            mockPlayerCardsInfo.playerCards.DidNotReceive().Remove(Arg.Any<CardInfoSerialized.CardInfoStruct>());
        }

        [Test]
        public void ShuffleDeck_WithShuffle_ShufflesTheDeck()
        {
            // Arrange
            var cardData = ScriptableObject.CreateInstance<CardsDataBase>();
            var cardInfo1 = new CardInfoSerialized.CardInfoStruct { Id = 1, CardName = "Card1" };
            var cardInfo2 = new CardInfoSerialized.CardInfoStruct { Id = 2, CardName = "Card2" };
            var cardInfo3 = new CardInfoSerialized.CardInfoStruct { Id = 3, CardName = "Card3" };
            cardData.cardDataBase.Sheet1 = new List<CardInfoSerialized.CardInfoStruct>
                { cardInfo1, cardInfo2, cardInfo3 };
            _mockGameManager.GetCardFromDataBaseByIndex(Arg.Any<int>()).Returns(cardInfo1, cardInfo2, cardInfo3);

            // Act
            PlayerController systemUnderTest = CreateSystem();
            systemUnderTest.ShuffleDeck(false, true);

            // Assert
            CollectionAssert.AreNotEqual(
                new List<CardInfoSerialized.CardInfoStruct> { cardInfo1, cardInfo2, cardInfo3 },
                systemUnderTest.Debug_GetShuffledDeck().playerCards);
        }

        [Test]
        public void ShuffleDeck_RemovesPilotCard()
        {
            // Arrange
            var cardData = ScriptableObject.CreateInstance<CardsDataBase>();
            var pilotCard = new CardInfoSerialized.CardInfoStruct
                { Id = 0, CardName = "PilotCard", TypeEnum = CardType.Pilot };
            var otherCard = new CardInfoSerialized.CardInfoStruct
                { Id = 1, CardName = "OtherCard", TypeEnum = CardType.Weapon };
            cardData.cardDataBase.Sheet1 = new List<CardInfoSerialized.CardInfoStruct> { pilotCard, otherCard };
            _mockGameManager.GetCardFromDataBaseByIndex(Arg.Any<int>()).Returns(pilotCard, otherCard);

            // Act
            PlayerController systemUnderTest = CreateSystem();
            systemUnderTest.ShuffleDeck(true, true);

            // Assert
            CollectionAssert.DoesNotContain(systemUnderTest.Debug_GetShuffledDeck().playerCards, pilotCard);
        }

        #endregion
    }
}
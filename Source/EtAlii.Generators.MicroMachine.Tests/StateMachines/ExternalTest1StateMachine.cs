// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameterInPartialMethod
namespace EtAlii.Generators.MicroMachine.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public partial class ExternalTest1StateMachine
    {
        public List<string> Transitions { get; } = new();

        private void LogTransition(Type triggerType, [CallerMemberName] string methodName = null) => Transitions.Add($"{methodName}({triggerType.Name} trigger)");

        private partial void OnStartEntered(Trigger trigger, StartChoices choices) => LogTransition(typeof(Trigger));
        private partial void OnStartEntered(StartTrigger trigger, StartChoices choices) => LogTransition(typeof(StartTrigger));
        private partial void OnStartExited(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));
        private partial void OnStartExited(Trigger trigger) => LogTransition(typeof(Trigger));


        private partial void OnGetSituationEntered(Trigger trigger, GetSituationChoices choices) => LogTransition(typeof(Trigger));
        private partial void OnGetSituationEntered(ContinueTrigger trigger, GetSituationChoices choices) => LogTransition(typeof(ContinueTrigger));
        private partial void OnGetSituationExited(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));
        private partial void OnGetSituationExited(ErrorTrigger trigger) => LogTransition(typeof(ErrorTrigger));
        private partial void OnGetSituationExited(Trigger trigger) => LogTransition(typeof(Trigger));


        private partial void OnDetermineCoinToBetOnEntered(Trigger trigger, DetermineCoinToBetOnChoices choices) => LogTransition(typeof(Trigger));
        private partial void OnDetermineCoinToBetOnEntered(ContinueTrigger trigger, DetermineCoinToBetOnChoices choices) => LogTransition(typeof(ContinueTrigger));
        private partial void OnDetermineCoinToBetOnExited(CurrentCoinHasBestTrendTrigger trigger) => LogTransition(typeof(CurrentCoinHasBestTrendTrigger));
        private partial void OnDetermineCoinToBetOnExited(OtherCoinHasBetterTrendTrigger trigger) => LogTransition(typeof(OtherCoinHasBetterTrendTrigger));
        private partial void OnDetermineCoinToBetOnExited(AllCoinsHaveDownwardTrendsTrigger trigger) => LogTransition(typeof(AllCoinsHaveDownwardTrendsTrigger));
        private partial void OnDetermineCoinToBetOnExited(NoPreviousCoinTrigger trigger) => LogTransition(typeof(NoPreviousCoinTrigger));
        private partial void OnDetermineCoinToBetOnExited(Trigger trigger) => LogTransition(typeof(Trigger));


        private partial void OnDetermineSymbolPairEntered(Trigger trigger, DetermineSymbolPairChoices choices) => LogTransition(typeof(Trigger));
        private partial void OnDetermineSymbolPairEntered(_IdleToDetermineSymbolPairTrigger trigger, DetermineSymbolPairChoices choices) => LogTransition(typeof(_IdleToDetermineSymbolPairTrigger));
        private partial void OnDetermineSymbolPairExited(IsNoSymbolPairTrigger trigger) => LogTransition(typeof(IsNoSymbolPairTrigger));
        private partial void OnDetermineSymbolPairExited(Trigger trigger) => LogTransition(typeof(Trigger));


        private partial void On_IdleEntered(Trigger trigger, _IdleChoices choices) => LogTransition(typeof(Trigger));

        private partial void On_IdleExited(Trigger trigger) => LogTransition(typeof(Trigger));

        private partial void On_IdleExited(StartTrigger trigger) => LogTransition(typeof(StartTrigger));

        private partial void On_IdleEntered(WaitUntilCoinSoldInUsdtTransferTo_IdleTrigger trigger, _IdleChoices choices) => LogTransition(typeof(WaitUntilCoinSoldInUsdtTransferTo_IdleTrigger));

        private partial void On_IdleEntered(WaitUntilCoinBoughtInUsdtTransferTo_IdleTrigger trigger, _IdleChoices choices) => LogTransition(typeof(WaitUntilCoinBoughtInUsdtTransferTo_IdleTrigger));

        private partial void On_IdleEntered(WaitUntilCoinSoldAsSymbolPairTo_IdleTrigger trigger, _IdleChoices choices) => LogTransition(typeof(WaitUntilCoinSoldAsSymbolPairTo_IdleTrigger));

        private partial void On_IdleEntered(WaitUntilCoinBoughtTo_IdleTrigger trigger, _IdleChoices choices) => LogTransition(typeof(WaitUntilCoinBoughtTo_IdleTrigger));

        private partial void OnBuyCurrentCoinInUsdtTransferEntered(Trigger trigger, BuyCurrentCoinInUsdtTransferChoices choices) => LogTransition(typeof(Trigger));

        private partial void OnBuyCurrentCoinInUsdtTransferExited(Trigger trigger) => LogTransition(typeof(Trigger));

        private partial void OnBuyCurrentCoinInUsdtTransferEntered(_IdleToBuyCurrentCoinInUsdtTransferTrigger trigger, BuyCurrentCoinInUsdtTransferChoices choices) => LogTransition(typeof(_IdleToBuyCurrentCoinInUsdtTransferTrigger));

        private partial void OnBuyCurrentCoinInUsdtTransferExited(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));

        private partial void OnBuyOtherCoinEntered(Trigger trigger, BuyOtherCoinChoices choices) => LogTransition(typeof(Trigger));

        private partial void OnBuyOtherCoinExited(Trigger trigger) => LogTransition(typeof(Trigger));

        private partial void OnBuyOtherCoinEntered(ContinueTrigger trigger, BuyOtherCoinChoices choices) => LogTransition(typeof(ContinueTrigger));

        private partial void OnBuyOtherCoinExited(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));

        private partial void OnDetermineSymbolPairExited(IsSymbolPairTrigger trigger) => LogTransition(typeof(IsSymbolPairTrigger));

        private partial void OnSellAsSymbolPairEntered(Trigger trigger, SellAsSymbolPairChoices choices) => LogTransition(typeof(Trigger));

        private partial void OnSellAsSymbolPairExited(Trigger trigger) => LogTransition(typeof(Trigger));

        private partial void OnSellAsSymbolPairEntered(IsSymbolPairTrigger trigger, SellAsSymbolPairChoices choices) => LogTransition(typeof(IsSymbolPairTrigger));

        private partial void OnSellAsSymbolPairExited(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));

        private partial void OnSellCurrentCoinEntered(Trigger trigger, SellCurrentCoinChoices choices) => LogTransition(typeof(Trigger));

        private partial void OnSellCurrentCoinExited(Trigger trigger) => LogTransition(typeof(Trigger));

        private partial void OnSellCurrentCoinEntered(IsNoSymbolPairTrigger trigger, SellCurrentCoinChoices choices) => LogTransition(typeof(IsNoSymbolPairTrigger));

        private partial void OnSellCurrentCoinExited(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));

        private partial void OnSellCurrentCoinInUsdtTransferEntered(Trigger trigger, SellCurrentCoinInUsdtTransferChoices choices) => LogTransition(typeof(Trigger));

        private partial void OnSellCurrentCoinInUsdtTransferExited(Trigger trigger) => LogTransition(typeof(Trigger));

        private partial void OnSellCurrentCoinInUsdtTransferEntered(_IdleToSellCurrentCoinInUsdtTransferTrigger trigger, SellCurrentCoinInUsdtTransferChoices choices) => LogTransition(typeof(_IdleToSellCurrentCoinInUsdtTransferTrigger));

        private partial void OnSellCurrentCoinInUsdtTransferExited(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));

        private partial void OnTransferFromUsdtEntered(Trigger trigger, TransferFromUsdtChoices choices) => LogTransition(typeof(Trigger));

        private partial void OnTransferFromUsdtExited(Trigger trigger) => LogTransition(typeof(Trigger));

        private partial void OnTransferFromUsdtEntered(NoPreviousCoinTrigger trigger, TransferFromUsdtChoices choices) => LogTransition(typeof(NoPreviousCoinTrigger));

        private partial void OnTransferFromUsdtExited(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));

        private partial void OnTransferToOtherCoinEntered(Trigger trigger, TransferToOtherCoinChoices choices) => LogTransition(typeof(Trigger));

        private partial void OnTransferToOtherCoinExited(Trigger trigger) => LogTransition(typeof(Trigger));

        private partial void OnTransferToOtherCoinEntered(OtherCoinHasBetterTrendTrigger trigger, TransferToOtherCoinChoices choices) => LogTransition(typeof(OtherCoinHasBetterTrendTrigger));

        private partial void OnTransferToOtherCoinExited(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));

        private partial void OnTransferToUsdtEntered(Trigger trigger, TransferToUsdtChoices choices) => LogTransition(typeof(Trigger));

        private partial void OnTransferToUsdtExited(Trigger trigger) => LogTransition(typeof(Trigger));

        private partial void OnTransferToUsdtEntered(AllCoinsHaveDownwardTrendsTrigger trigger, TransferToUsdtChoices choices) => LogTransition(typeof(AllCoinsHaveDownwardTrendsTrigger));

        private partial void OnTransferToUsdtExited(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));


        private partial void OnWaitEntered(Trigger trigger, WaitChoices choices) => LogTransition(typeof(Trigger));
        private partial void OnWaitEntered(ErrorTrigger trigger, WaitChoices choices) => LogTransition(typeof(ErrorTrigger));
        private partial void OnWaitEntered(CurrentCoinHasBestTrendTrigger trigger, WaitChoices choices) => LogTransition(typeof(CurrentCoinHasBestTrendTrigger));
        private partial void OnWaitEntered(ContinueTrigger trigger, WaitChoices choices) => LogTransition(typeof(ContinueTrigger));
        private partial void OnWaitExited(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));
        private partial void OnWaitExited(Trigger trigger) => LogTransition(typeof(Trigger));


        private partial void OnWaitUntilCoinBoughtEntered(Trigger trigger, WaitUntilCoinBoughtChoices choices) => LogTransition(typeof(Trigger));

        private partial void OnWaitUntilCoinBoughtExited(Trigger trigger) => LogTransition(typeof(Trigger));

        private partial void OnWaitUntilCoinBoughtEntered(ContinueTrigger trigger, WaitUntilCoinBoughtChoices choices) => LogTransition(typeof(ContinueTrigger));

        private partial void OnWaitUntilCoinBoughtExited(WaitUntilCoinBoughtTo_IdleTrigger trigger) => LogTransition(typeof(WaitUntilCoinBoughtTo_IdleTrigger));

        private partial void OnWaitUntilCoinBoughtInUsdtTransferEntered(Trigger trigger, WaitUntilCoinBoughtInUsdtTransferChoices choices) => LogTransition(typeof(Trigger));

        private partial void OnWaitUntilCoinBoughtInUsdtTransferExited(Trigger trigger) => LogTransition(typeof(Trigger));

        private partial void OnWaitUntilCoinBoughtInUsdtTransferEntered(ContinueTrigger trigger, WaitUntilCoinBoughtInUsdtTransferChoices choices) => LogTransition(typeof(ContinueTrigger));

        private partial void OnWaitUntilCoinBoughtInUsdtTransferExited(WaitUntilCoinBoughtInUsdtTransferTo_IdleTrigger trigger) => LogTransition(typeof(WaitUntilCoinBoughtInUsdtTransferTo_IdleTrigger));

        private partial void OnWaitUntilCoinSoldEntered(Trigger trigger, WaitUntilCoinSoldChoices choices) => LogTransition(typeof(Trigger));

        private partial void OnWaitUntilCoinSoldExited(Trigger trigger) => LogTransition(typeof(Trigger));

        private partial void OnWaitUntilCoinSoldEntered(ContinueTrigger trigger, WaitUntilCoinSoldChoices choices) => LogTransition(typeof(ContinueTrigger));

        private partial void OnWaitUntilCoinSoldExited(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));

        private partial void OnWaitUntilCoinSoldAsSymbolPairEntered(Trigger trigger, WaitUntilCoinSoldAsSymbolPairChoices choices) => LogTransition(typeof(Trigger));

        private partial void OnWaitUntilCoinSoldAsSymbolPairExited(Trigger trigger) => LogTransition(typeof(Trigger));

        private partial void OnWaitUntilCoinSoldAsSymbolPairEntered(ContinueTrigger trigger, WaitUntilCoinSoldAsSymbolPairChoices choices) => LogTransition(typeof(ContinueTrigger));

        private partial void OnWaitUntilCoinSoldAsSymbolPairExited(WaitUntilCoinSoldAsSymbolPairTo_IdleTrigger trigger) => LogTransition(typeof(WaitUntilCoinSoldAsSymbolPairTo_IdleTrigger));

        private partial void OnWaitUntilCoinSoldInUsdtTransferEntered(Trigger trigger, WaitUntilCoinSoldInUsdtTransferChoices choices) => LogTransition(typeof(Trigger));

        private partial void OnWaitUntilCoinSoldInUsdtTransferExited(Trigger trigger) => LogTransition(typeof(Trigger));

        private partial void OnWaitUntilCoinSoldInUsdtTransferEntered(ContinueTrigger trigger, WaitUntilCoinSoldInUsdtTransferChoices choices) => LogTransition(typeof(ContinueTrigger));

        private partial void OnWaitUntilCoinSoldInUsdtTransferExited(WaitUntilCoinSoldInUsdtTransferTo_IdleTrigger trigger) => LogTransition(typeof(WaitUntilCoinSoldInUsdtTransferTo_IdleTrigger));

        private partial void On_IdleExited(ContinueTrigger trigger) => LogTransition(typeof(Trigger));

        private partial void On_IdleExited(_IdleToSellCurrentCoinInUsdtTransferTrigger trigger) => LogTransition(typeof(_IdleToSellCurrentCoinInUsdtTransferTrigger));

        private partial void On_IdleExited(_IdleToBuyCurrentCoinInUsdtTransferTrigger trigger) => LogTransition(typeof(_IdleToBuyCurrentCoinInUsdtTransferTrigger));

        private partial void On_IdleExited(_IdleToDetermineSymbolPairTrigger trigger) => LogTransition(typeof(_IdleToDetermineSymbolPairTrigger));

        private partial void OnBuyOtherCoinExited(_IdleToDetermineSymbolPairTrigger trigger) => LogTransition(typeof(_IdleToDetermineSymbolPairTrigger));

        private partial void OnDetermineSymbolPairExited(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));

        private partial void OnDetermineSymbolPairExited(_IdleToDetermineSymbolPairTrigger trigger) => LogTransition(typeof(_IdleToDetermineSymbolPairTrigger));

        private partial void OnSellCurrentCoinExited(_IdleToDetermineSymbolPairTrigger trigger) => LogTransition(typeof(_IdleToDetermineSymbolPairTrigger));

        private partial void OnSellAsSymbolPairExited(_IdleToDetermineSymbolPairTrigger trigger) => LogTransition(typeof(_IdleToDetermineSymbolPairTrigger));

        private partial void OnWaitUntilCoinBoughtExited(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));

        private partial void OnWaitUntilCoinBoughtExited(_IdleToDetermineSymbolPairTrigger trigger) => LogTransition(typeof(_IdleToDetermineSymbolPairTrigger));

        private partial void OnWaitUntilCoinSoldExited(_IdleToDetermineSymbolPairTrigger trigger) => LogTransition(typeof(_IdleToDetermineSymbolPairTrigger));

        private partial void OnBuyCurrentCoinInUsdtTransferExited(_IdleToBuyCurrentCoinInUsdtTransferTrigger trigger) => LogTransition(typeof(_IdleToBuyCurrentCoinInUsdtTransferTrigger));

        private partial void OnSellCurrentCoinInUsdtTransferExited(_IdleToSellCurrentCoinInUsdtTransferTrigger trigger) => LogTransition(typeof(_IdleToSellCurrentCoinInUsdtTransferTrigger));

        private partial void OnWaitUntilCoinBoughtInUsdtTransferExited(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));

        private partial void OnWaitUntilCoinBoughtInUsdtTransferExited(_IdleToBuyCurrentCoinInUsdtTransferTrigger trigger) => LogTransition(typeof(_IdleToBuyCurrentCoinInUsdtTransferTrigger));

        private partial void OnWaitUntilCoinSoldAsSymbolPairExited(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));

        private partial void OnWaitUntilCoinSoldAsSymbolPairExited(_IdleToDetermineSymbolPairTrigger trigger) => LogTransition(typeof(_IdleToDetermineSymbolPairTrigger));

        private partial void OnWaitUntilCoinSoldInUsdtTransferExited(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));

        private partial void OnWaitUntilCoinSoldInUsdtTransferExited(_IdleToSellCurrentCoinInUsdtTransferTrigger trigger) => LogTransition(typeof(_IdleToSellCurrentCoinInUsdtTransferTrigger));

        private partial void OnTransferFromUsdtEntered(_IdleToBuyCurrentCoinInUsdtTransferTrigger trigger, TransferFromUsdtChoices choices) => LogTransition(typeof(_IdleToBuyCurrentCoinInUsdtTransferTrigger));

        private partial void OnTransferFromUsdtExited(WaitUntilCoinBoughtInUsdtTransferTo_IdleTrigger trigger) => LogTransition(typeof(WaitUntilCoinBoughtInUsdtTransferTo_IdleTrigger));

        private partial void OnTransferToUsdtEntered(_IdleToSellCurrentCoinInUsdtTransferTrigger trigger, TransferToUsdtChoices choices) => LogTransition(typeof(_IdleToSellCurrentCoinInUsdtTransferTrigger));

        private partial void OnTransferToUsdtExited(WaitUntilCoinSoldInUsdtTransferTo_IdleTrigger trigger) => LogTransition(typeof(WaitUntilCoinSoldInUsdtTransferTo_IdleTrigger));

        private partial void OnTransferToOtherCoinEntered(_IdleToDetermineSymbolPairTrigger trigger, TransferToOtherCoinChoices choices) => LogTransition(typeof(_IdleToDetermineSymbolPairTrigger));

        private partial void OnTransferToOtherCoinExited(WaitUntilCoinBoughtTo_IdleTrigger trigger) => LogTransition(typeof(WaitUntilCoinBoughtTo_IdleTrigger));

        private partial void OnTransferToOtherCoinExited(WaitUntilCoinSoldAsSymbolPairTo_IdleTrigger trigger) => LogTransition(typeof(WaitUntilCoinSoldAsSymbolPairTo_IdleTrigger));
    }
}

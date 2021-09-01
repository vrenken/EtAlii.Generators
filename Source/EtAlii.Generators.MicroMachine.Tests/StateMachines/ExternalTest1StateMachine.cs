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

        partial void On_BeginEntered(Trigger trigger) => LogTransition(typeof(Trigger));

        partial void On_BeginExited(Trigger trigger) => LogTransition(typeof(Trigger));

        partial void On_BeginExited(StartTrigger trigger) => LogTransition(typeof(StartTrigger));

        partial void On_EndEntered(Trigger trigger) => LogTransition(typeof(Trigger));

        partial void On_EndExited(Trigger trigger) => LogTransition(typeof(Trigger));

        partial void On_EndEntered(WaitUntilCoinSoldInUsdtTransferTo_EndTrigger trigger) => LogTransition(typeof(WaitUntilCoinSoldInUsdtTransferTo_EndTrigger));

        partial void On_EndEntered(WaitUntilCoinBoughtInUsdtTransferTo_EndTrigger trigger) => LogTransition(typeof(WaitUntilCoinBoughtInUsdtTransferTo_EndTrigger));

        partial void On_EndEntered(WaitUntilCoinSoldAsSymbolPairTo_EndTrigger trigger) => LogTransition(typeof(WaitUntilCoinSoldAsSymbolPairTo_EndTrigger));

        partial void On_EndEntered(WaitUntilCoinBoughtTo_EndTrigger trigger) => LogTransition(typeof(WaitUntilCoinBoughtTo_EndTrigger));

        partial void OnBuyCurrentCoinInUsdtTransferEntered(Trigger trigger) => LogTransition(typeof(Trigger));

        partial void OnBuyCurrentCoinInUsdtTransferExited(Trigger trigger) => LogTransition(typeof(Trigger));

        partial void OnBuyCurrentCoinInUsdtTransferEntered(_BeginToBuyCurrentCoinInUsdtTransferTrigger trigger) => LogTransition(typeof(_BeginToBuyCurrentCoinInUsdtTransferTrigger));

        partial void OnBuyCurrentCoinInUsdtTransferExited(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));

        partial void OnBuyOtherCoinEntered(Trigger trigger) => LogTransition(typeof(Trigger));

        partial void OnBuyOtherCoinExited(Trigger trigger) => LogTransition(typeof(Trigger));

        partial void OnBuyOtherCoinEntered(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));

        partial void OnBuyOtherCoinExited(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));

        partial void OnDetermineCoinToBetOnEntered(Trigger trigger) => LogTransition(typeof(Trigger));

        partial void OnDetermineCoinToBetOnExited(Trigger trigger) => LogTransition(typeof(Trigger));

        partial void OnDetermineCoinToBetOnEntered(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));

        partial void OnDetermineCoinToBetOnExited(CurrentCoinHasBestTrendTrigger trigger) => LogTransition(typeof(CurrentCoinHasBestTrendTrigger));

        partial void OnDetermineCoinToBetOnExited(OtherCoinHasBetterTrendTrigger trigger) => LogTransition(typeof(OtherCoinHasBetterTrendTrigger));

        partial void OnDetermineCoinToBetOnExited(AllCoinsHaveDownwardTrendsTrigger trigger) => LogTransition(typeof(AllCoinsHaveDownwardTrendsTrigger));

        partial void OnDetermineCoinToBetOnExited(NoPreviousCoinTrigger trigger) => LogTransition(typeof(NoPreviousCoinTrigger));

        partial void OnDetermineSymbolPairEntered(Trigger trigger) => LogTransition(typeof(Trigger));

        partial void OnDetermineSymbolPairExited(Trigger trigger) => LogTransition(typeof(Trigger));

        partial void OnDetermineSymbolPairEntered(_BeginToDetermineSymbolPairTrigger trigger) => LogTransition(typeof(_BeginToDetermineSymbolPairTrigger));

        partial void OnDetermineSymbolPairExited(IsNoSymbolPairTrigger trigger) => LogTransition(typeof(IsNoSymbolPairTrigger));

        partial void OnDetermineSymbolPairExited(IsSymbolPairTrigger trigger) => LogTransition(typeof(IsSymbolPairTrigger));

        partial void OnGetSituationEntered(Trigger trigger) => LogTransition(typeof(Trigger));

        partial void OnGetSituationExited(Trigger trigger) => LogTransition(typeof(Trigger));

        partial void OnGetSituationEntered(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));

        partial void OnGetSituationExited(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));

        partial void OnGetSituationExited(ErrorTrigger trigger) => LogTransition(typeof(ErrorTrigger));

        partial void OnSellAsSymbolPairEntered(Trigger trigger) => LogTransition(typeof(Trigger));

        partial void OnSellAsSymbolPairExited(Trigger trigger) => LogTransition(typeof(Trigger));

        partial void OnSellAsSymbolPairEntered(IsSymbolPairTrigger trigger) => LogTransition(typeof(IsSymbolPairTrigger));

        partial void OnSellAsSymbolPairExited(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));

        partial void OnSellCurrentCoinEntered(Trigger trigger) => LogTransition(typeof(Trigger));

        partial void OnSellCurrentCoinExited(Trigger trigger) => LogTransition(typeof(Trigger));

        partial void OnSellCurrentCoinEntered(IsNoSymbolPairTrigger trigger) => LogTransition(typeof(IsNoSymbolPairTrigger));

        partial void OnSellCurrentCoinExited(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));

        partial void OnSellCurrentCoinInUsdtTransferEntered(Trigger trigger) => LogTransition(typeof(Trigger));

        partial void OnSellCurrentCoinInUsdtTransferExited(Trigger trigger) => LogTransition(typeof(Trigger));

        partial void OnSellCurrentCoinInUsdtTransferEntered(_BeginToSellCurrentCoinInUsdtTransferTrigger trigger) => LogTransition(typeof(_BeginToSellCurrentCoinInUsdtTransferTrigger));

        partial void OnSellCurrentCoinInUsdtTransferExited(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));

        partial void OnStartEntered(Trigger trigger) => LogTransition(typeof(Trigger));

        partial void OnStartExited(Trigger trigger) => LogTransition(typeof(Trigger));

        partial void OnStartEntered(StartTrigger trigger) => LogTransition(typeof(StartTrigger));

        partial void OnStartExited(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));

        partial void OnTransferFromUsdtEntered(Trigger trigger) => LogTransition(typeof(Trigger));

        partial void OnTransferFromUsdtExited(Trigger trigger) => LogTransition(typeof(Trigger));

        partial void OnTransferFromUsdtEntered(NoPreviousCoinTrigger trigger) => LogTransition(typeof(NoPreviousCoinTrigger));

        partial void OnTransferFromUsdtExited(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));

        partial void OnTransferToOtherCoinEntered(Trigger trigger) => LogTransition(typeof(Trigger));

        partial void OnTransferToOtherCoinExited(Trigger trigger) => LogTransition(typeof(Trigger));

        partial void OnTransferToOtherCoinEntered(OtherCoinHasBetterTrendTrigger trigger) => LogTransition(typeof(OtherCoinHasBetterTrendTrigger));

        partial void OnTransferToOtherCoinExited(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));

        partial void OnTransferToUsdtEntered(Trigger trigger) => LogTransition(typeof(Trigger));

        partial void OnTransferToUsdtExited(Trigger trigger) => LogTransition(typeof(Trigger));

        partial void OnTransferToUsdtEntered(AllCoinsHaveDownwardTrendsTrigger trigger) => LogTransition(typeof(AllCoinsHaveDownwardTrendsTrigger));

        partial void OnTransferToUsdtExited(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));

        partial void OnWaitEntered(Trigger trigger) => LogTransition(typeof(Trigger));

        partial void OnWaitExited(Trigger trigger) => LogTransition(typeof(Trigger));

        partial void OnWaitEntered(ErrorTrigger trigger) => LogTransition(typeof(ErrorTrigger));

        partial void OnWaitEntered(CurrentCoinHasBestTrendTrigger trigger) => LogTransition(typeof(CurrentCoinHasBestTrendTrigger));

        partial void OnWaitEntered(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));

        partial void OnWaitExited(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));

        partial void OnWaitUntilCoinBoughtEntered(Trigger trigger) => LogTransition(typeof(Trigger));

        partial void OnWaitUntilCoinBoughtExited(Trigger trigger) => LogTransition(typeof(Trigger));

        partial void OnWaitUntilCoinBoughtEntered(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));

        partial void OnWaitUntilCoinBoughtExited(WaitUntilCoinBoughtTo_EndTrigger trigger) => LogTransition(typeof(WaitUntilCoinBoughtTo_EndTrigger));

        partial void OnWaitUntilCoinBoughtInUsdtTransferEntered(Trigger trigger) => LogTransition(typeof(Trigger));

        partial void OnWaitUntilCoinBoughtInUsdtTransferExited(Trigger trigger) => LogTransition(typeof(Trigger));

        partial void OnWaitUntilCoinBoughtInUsdtTransferEntered(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));

        partial void OnWaitUntilCoinBoughtInUsdtTransferExited(WaitUntilCoinBoughtInUsdtTransferTo_EndTrigger trigger) => LogTransition(typeof(WaitUntilCoinBoughtInUsdtTransferTo_EndTrigger));

        partial void OnWaitUntilCoinSoldEntered(Trigger trigger) => LogTransition(typeof(Trigger));

        partial void OnWaitUntilCoinSoldExited(Trigger trigger) => LogTransition(typeof(Trigger));

        partial void OnWaitUntilCoinSoldEntered(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));

        partial void OnWaitUntilCoinSoldExited(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));

        partial void OnWaitUntilCoinSoldAsSymbolPairEntered(Trigger trigger) => LogTransition(typeof(Trigger));

        partial void OnWaitUntilCoinSoldAsSymbolPairExited(Trigger trigger) => LogTransition(typeof(Trigger));

        partial void OnWaitUntilCoinSoldAsSymbolPairEntered(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));

        partial void OnWaitUntilCoinSoldAsSymbolPairExited(WaitUntilCoinSoldAsSymbolPairTo_EndTrigger trigger) => LogTransition(typeof(WaitUntilCoinSoldAsSymbolPairTo_EndTrigger));

        partial void OnWaitUntilCoinSoldInUsdtTransferEntered(Trigger trigger) => LogTransition(typeof(Trigger));

        partial void OnWaitUntilCoinSoldInUsdtTransferExited(Trigger trigger) => LogTransition(typeof(Trigger));

        partial void OnWaitUntilCoinSoldInUsdtTransferEntered(ContinueTrigger trigger) => LogTransition(typeof(ContinueTrigger));

        partial void OnWaitUntilCoinSoldInUsdtTransferExited(WaitUntilCoinSoldInUsdtTransferTo_EndTrigger trigger) => LogTransition(typeof(WaitUntilCoinSoldInUsdtTransferTo_EndTrigger));
    }
}

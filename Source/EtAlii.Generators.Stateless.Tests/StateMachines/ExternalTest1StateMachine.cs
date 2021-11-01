// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameterInPartialMethod
namespace EtAlii.Generators.Stateless.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public partial class ExternalTest1StateMachine
    {
        public List<string> Transitions { get; } = new();

        private void LogTransition(Type eventArgsType = null, [CallerMemberName] string methodName = null)
        {
            var parameters = eventArgsType != null ? $"{eventArgsType.Name} e" : string.Empty;
            Transitions.Add($"{methodName}({parameters})");
        }

        partial void On_BeginEntered(_BeginEventArgs e) => LogTransition();

        partial void On_BeginExited() => LogTransition();

        partial void On_EndEntered(_EndEventArgs e) => LogTransition();

        partial void On_EndExited() => LogTransition();

        partial void On_EndEnteredFromWaitUntilCoinSoldInUsdtTransferTo_EndTrigger(_EndEventArgs e) => LogTransition();

        partial void On_EndEnteredFromWaitUntilCoinBoughtInUsdtTransferTo_EndTrigger(_EndEventArgs e) => LogTransition();

        partial void On_EndEnteredFromWaitUntilCoinSoldAsSymbolPairTo_EndTrigger(_EndEventArgs e) => LogTransition();

        partial void On_EndEnteredFromWaitUntilCoinBoughtTo_EndTrigger(_EndEventArgs e) => LogTransition();

        partial void OnBuyCurrentCoinInUsdtTransferEntered(BuyCurrentCoinInUsdtTransferEventArgs e) => LogTransition();

        partial void OnBuyCurrentCoinInUsdtTransferExited() => LogTransition();

        partial void OnBuyCurrentCoinInUsdtTransferEnteredFrom_BeginToBuyCurrentCoinInUsdtTransferTrigger(BuyCurrentCoinInUsdtTransferEventArgs e) => LogTransition();

        partial void OnBuyOtherCoinEntered(BuyOtherCoinEventArgs e) => LogTransition();

        partial void OnBuyOtherCoinExited() => LogTransition();

        partial void OnBuyOtherCoinEnteredFromContinueTrigger(BuyOtherCoinEventArgs e) => LogTransition();

        partial void OnDetermineCoinToBetOnEntered(DetermineCoinToBetOnEventArgs e) => LogTransition(typeof(DetermineCoinToBetOnEventArgs));

        partial void OnDetermineCoinToBetOnExited() => LogTransition();

        partial void OnDetermineCoinToBetOnEnteredFromContinueTrigger(DetermineCoinToBetOnEventArgs e) => LogTransition(typeof(DetermineCoinToBetOnEventArgs));

        partial void OnDetermineSymbolPairEntered(DetermineSymbolPairEventArgs e) => LogTransition(typeof(DetermineSymbolPairEventArgs));

        partial void OnDetermineSymbolPairExited() => LogTransition();

        partial void OnDetermineSymbolPairEnteredFrom_BeginToDetermineSymbolPairTrigger(DetermineSymbolPairEventArgs e) => LogTransition(typeof(DetermineSymbolPairEventArgs));

        partial void OnGetSituationEntered(GetSituationEventArgs e) => LogTransition();

        partial void OnGetSituationExited() => LogTransition();

        partial void OnGetSituationEnteredFromContinueTrigger(GetSituationEventArgs e) => LogTransition();

        partial void OnSellAsSymbolPairEntered(SellAsSymbolPairEventArgs e) => LogTransition();

        partial void OnSellAsSymbolPairExited() => LogTransition();

        partial void OnSellAsSymbolPairEnteredFromIsSymbolPairTrigger(SellAsSymbolPairEventArgs e) => LogTransition();

        partial void OnSellCurrentCoinEntered(SellCurrentCoinEventArgs e) => LogTransition();

        partial void OnSellCurrentCoinExited() => LogTransition();

        partial void OnSellCurrentCoinEnteredFromIsNoSymbolPairTrigger(SellCurrentCoinEventArgs e) => LogTransition();

        partial void OnSellCurrentCoinInUsdtTransferEntered(SellCurrentCoinInUsdtTransferEventArgs e) => LogTransition();

        partial void OnSellCurrentCoinInUsdtTransferExited() => LogTransition();

        partial void OnSellCurrentCoinInUsdtTransferEnteredFrom_BeginToSellCurrentCoinInUsdtTransferTrigger(SellCurrentCoinInUsdtTransferEventArgs e) => LogTransition();

        partial void OnStartEntered(StartEventArgs e) => LogTransition();

        partial void OnStartExited() => LogTransition();

        partial void OnStartEnteredFromStartTrigger(StartEventArgs e) => LogTransition();

        partial void OnTransferFromUsdtEntered(TransferFromUsdtEventArgs e) => LogTransition();

        partial void OnTransferFromUsdtExited() => LogTransition();

        partial void OnTransferFromUsdtEnteredFromNoPreviousCoinTrigger(TransferFromUsdtEventArgs e) => LogTransition();

        partial void OnTransferToOtherCoinEntered(TransferToOtherCoinEventArgs e) => LogTransition();

        partial void OnTransferToOtherCoinExited() => LogTransition();

        partial void OnTransferToOtherCoinEnteredFromOtherCoinHasBetterTrendTrigger(TransferToOtherCoinEventArgs e) => LogTransition();

        partial void OnTransferToUsdtEntered(TransferToUsdtEventArgs e) => LogTransition();

        partial void OnTransferToUsdtExited() => LogTransition();

        partial void OnTransferToUsdtEnteredFromAllCoinsHaveDownwardTrendsTrigger(TransferToUsdtEventArgs e) => LogTransition();

        partial void OnWaitEntered(WaitEventArgs e) => LogTransition();

        partial void OnWaitExited() => LogTransition();

        partial void OnWaitEnteredFromErrorTrigger(WaitEventArgs e) => LogTransition();

        partial void OnWaitEnteredFromCurrentCoinHasBestTrendTrigger(WaitEventArgs e) => LogTransition();

        partial void OnWaitEnteredFromContinueTrigger(WaitEventArgs e) => LogTransition();

        partial void OnWaitUntilCoinBoughtEntered(WaitUntilCoinBoughtEventArgs e) => LogTransition();

        partial void OnWaitUntilCoinBoughtExited() => LogTransition();

        partial void OnWaitUntilCoinBoughtEnteredFromContinueTrigger(WaitUntilCoinBoughtEventArgs e) => LogTransition();

        partial void OnWaitUntilCoinBoughtInUsdtTransferEntered(WaitUntilCoinBoughtInUsdtTransferEventArgs e) => LogTransition();

        partial void OnWaitUntilCoinBoughtInUsdtTransferExited() => LogTransition();

        partial void OnWaitUntilCoinBoughtInUsdtTransferEnteredFromContinueTrigger(WaitUntilCoinBoughtInUsdtTransferEventArgs e) => LogTransition();

        partial void OnWaitUntilCoinSoldEntered(WaitUntilCoinSoldEventArgs e) => LogTransition();

        partial void OnWaitUntilCoinSoldExited() => LogTransition();

        partial void OnWaitUntilCoinSoldEnteredFromContinueTrigger(WaitUntilCoinSoldEventArgs e) => LogTransition();

        partial void OnWaitUntilCoinSoldAsSymbolPairEntered(WaitUntilCoinSoldAsSymbolPairEventArgs e) => LogTransition();

        partial void OnWaitUntilCoinSoldAsSymbolPairExited() => LogTransition();

        partial void OnWaitUntilCoinSoldAsSymbolPairEnteredFromContinueTrigger(WaitUntilCoinSoldAsSymbolPairEventArgs e) => LogTransition();

        partial void OnWaitUntilCoinSoldInUsdtTransferEntered(WaitUntilCoinSoldInUsdtTransferEventArgs e) => LogTransition();

        partial void OnWaitUntilCoinSoldInUsdtTransferExited() => LogTransition();

        partial void OnWaitUntilCoinSoldInUsdtTransferEnteredFromContinueTrigger(WaitUntilCoinSoldInUsdtTransferEventArgs e) => LogTransition();
    }
}

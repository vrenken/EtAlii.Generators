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

        partial void On_BeginEntered() => LogTransition();

        partial void On_BeginExited() => LogTransition();

        partial void On_EndEntered() => LogTransition();

        partial void On_EndExited() => LogTransition();

        partial void On_EndEnteredFromWaitUntilCoinSoldInUsdtTransferTo_EndTrigger() => LogTransition();

        partial void On_EndEnteredFromWaitUntilCoinBoughtInUsdtTransferTo_EndTrigger() => LogTransition();

        partial void On_EndEnteredFromWaitUntilCoinSoldAsSymbolPairTo_EndTrigger() => LogTransition();

        partial void On_EndEnteredFromWaitUntilCoinBoughtTo_EndTrigger() => LogTransition();

        partial void OnBuyCurrentCoinInUsdtTransferEntered() => LogTransition();

        partial void OnBuyCurrentCoinInUsdtTransferExited() => LogTransition();

        partial void OnBuyCurrentCoinInUsdtTransferEnteredFrom_BeginToBuyCurrentCoinInUsdtTransferTrigger() => LogTransition();

        partial void OnBuyOtherCoinEntered() => LogTransition();

        partial void OnBuyOtherCoinExited() => LogTransition();

        partial void OnBuyOtherCoinEnteredFromContinueTrigger() => LogTransition();

        partial void OnDetermineCoinToBetOnEntered(DetermineCoinToBetOnEventArgs e) => LogTransition(typeof(DetermineCoinToBetOnEventArgs));

        partial void OnDetermineCoinToBetOnExited() => LogTransition();

        partial void OnDetermineCoinToBetOnEnteredFromContinueTrigger(DetermineCoinToBetOnEventArgs e) => LogTransition(typeof(DetermineCoinToBetOnEventArgs));

        partial void OnDetermineSymbolPairEntered(DetermineSymbolPairEventArgs e) => LogTransition(typeof(DetermineSymbolPairEventArgs));

        partial void OnDetermineSymbolPairExited() => LogTransition();

        partial void OnDetermineSymbolPairEnteredFrom_BeginToDetermineSymbolPairTrigger(DetermineSymbolPairEventArgs e) => LogTransition(typeof(DetermineSymbolPairEventArgs));

        partial void OnGetSituationEntered() => LogTransition();

        partial void OnGetSituationExited() => LogTransition();

        partial void OnGetSituationEnteredFromContinueTrigger() => LogTransition();

        partial void OnSellAsSymbolPairEntered() => LogTransition();

        partial void OnSellAsSymbolPairExited() => LogTransition();

        partial void OnSellAsSymbolPairEnteredFromIsSymbolPairTrigger() => LogTransition();

        partial void OnSellCurrentCoinEntered() => LogTransition();

        partial void OnSellCurrentCoinExited() => LogTransition();

        partial void OnSellCurrentCoinEnteredFromIsNoSymbolPairTrigger() => LogTransition();

        partial void OnSellCurrentCoinInUsdtTransferEntered() => LogTransition();

        partial void OnSellCurrentCoinInUsdtTransferExited() => LogTransition();

        partial void OnSellCurrentCoinInUsdtTransferEnteredFrom_BeginToSellCurrentCoinInUsdtTransferTrigger() => LogTransition();

        partial void OnStartEntered() => LogTransition();

        partial void OnStartExited() => LogTransition();

        partial void OnStartEnteredFromStartTrigger() => LogTransition();

        partial void OnTransferFromUsdtEntered() => LogTransition();

        partial void OnTransferFromUsdtExited() => LogTransition();

        partial void OnTransferFromUsdtEnteredFromNoPreviousCoinTrigger() => LogTransition();

        partial void OnTransferToOtherCoinEntered() => LogTransition();

        partial void OnTransferToOtherCoinExited() => LogTransition();

        partial void OnTransferToOtherCoinEnteredFromOtherCoinHasBetterTrendTrigger() => LogTransition();

        partial void OnTransferToUsdtEntered() => LogTransition();

        partial void OnTransferToUsdtExited() => LogTransition();

        partial void OnTransferToUsdtEnteredFromAllCoinsHaveDownwardTrendsTrigger() => LogTransition();

        partial void OnWaitEntered() => LogTransition();

        partial void OnWaitExited() => LogTransition();

        partial void OnWaitEnteredFromErrorTrigger() => LogTransition();

        partial void OnWaitEnteredFromCurrentCoinHasBestTrendTrigger() => LogTransition();

        partial void OnWaitEnteredFromContinueTrigger() => LogTransition();

        partial void OnWaitUntilCoinBoughtEntered() => LogTransition();

        partial void OnWaitUntilCoinBoughtExited() => LogTransition();

        partial void OnWaitUntilCoinBoughtEnteredFromContinueTrigger() => LogTransition();

        partial void OnWaitUntilCoinBoughtInUsdtTransferEntered() => LogTransition();

        partial void OnWaitUntilCoinBoughtInUsdtTransferExited() => LogTransition();

        partial void OnWaitUntilCoinBoughtInUsdtTransferEnteredFromContinueTrigger() => LogTransition();

        partial void OnWaitUntilCoinSoldEntered() => LogTransition();

        partial void OnWaitUntilCoinSoldExited() => LogTransition();

        partial void OnWaitUntilCoinSoldEnteredFromContinueTrigger() => LogTransition();

        partial void OnWaitUntilCoinSoldAsSymbolPairEntered() => LogTransition();

        partial void OnWaitUntilCoinSoldAsSymbolPairExited() => LogTransition();

        partial void OnWaitUntilCoinSoldAsSymbolPairEnteredFromContinueTrigger() => LogTransition();

        partial void OnWaitUntilCoinSoldInUsdtTransferEntered() => LogTransition();

        partial void OnWaitUntilCoinSoldInUsdtTransferExited() => LogTransition();

        partial void OnWaitUntilCoinSoldInUsdtTransferEnteredFromContinueTrigger() => LogTransition();
    }
}

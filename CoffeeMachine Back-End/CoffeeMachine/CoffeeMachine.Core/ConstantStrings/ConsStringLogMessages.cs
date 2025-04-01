namespace CoffeeMachine.Core.ConstantStrings
{
    public static class ConsStringLogMessages
    {
        public const string UnexpectedAction = "Unexpected action";
        public const string ErrorMakingCoffee = "Error making coffee.";
        public const string ErrorLoggingAction = "Error logging action";
        public const string ErrorRetrievingActionLogs = "Error retrieving coffee action logs";
        public const string ErrorRetrievingActionTypes = "Error retrieving action types configuration";
        public const string ErrorTurningOnMach = "Error turning on the coffee machine.";
        public const string ErrorTurningOffMach = "Error turning off the coffee machine.";
        public const string ErrorInitDB = "An error occurred while initializing the database.";

        public const string CreatingNewAction = "Creating new Action";
        public const string InvalidState = "Invalid state";
        public const string InvalidStateMachIsOn = "Invalid state: Machine is already on.";
        public const string InvalidStateMachStatus = "Invalid state: Machine is not on or is making coffee, or is in alert state. Check the machine status";
        public const string InvalidRequest = "Invalid request: Choose at least one option (milk , espresso, etc..)";
        public const string MachTunedOn = "Machine turned on.";
        public const string MachTunedOff = "Machine turned off.";
        public const string EspressoText = "Coffee made with options: Espresso Shots - ";
        public const string MilkText = "Milk - ";

        public const string FirstCup = "First Cup: ";
        public const string LastCup = "Last Cup: ";

        public const string MiddlewareExceptionMessage = "Unhandled exception occurred.";
        public const string MiddlewareGeneralErrorMessage = "An error occurred.";

    }
}

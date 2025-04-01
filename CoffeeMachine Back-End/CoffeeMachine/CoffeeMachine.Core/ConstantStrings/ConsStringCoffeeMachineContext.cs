namespace CoffeeMachine.Core.ConstantStrings
{
    public static class ConsStringCoffeeMachineContext
    {
        // Database table names
        public const string TableCoffeeActionLogs = "CoffeeActionLogs";
        public const string TableActions = "Actions";
        public const string TableCoffeeMachineStates = "CoffeeMachineStates";
        public const string TableActionTypes = "ActionTypes";
        public const string TableCoffeeCreationOptions = "CoffeeCreationOptions";

        // Column names
        public const string ColumnActionId = "ActionId";
        public const string ColumnActionTypeId = "ActionTypeId";
        public const string ColumnCoffeeCreationOptionsId = "CoffeeCreationOptionsId";
        public const string ColumnId = "Id";
        public const string ColumnActionName = "ActionName";
        public const string ColumnIsDisabledAction = "IsDisabledAction";
        public const string ColumnDescription = "Description";
        public const string ColumnTimestamp = "Timestamp";
        public const string ColumnDetails = "Details";
        public const string ColumnNumEspressoShots = "NumEspressoShots";
        public const string ColumnAddMilk = "AddMilk";
        public const string ColumnName = "Name";

        // ActionType seed data
        public static class ActionTypes
        {
            public const int TurnOffId = 1;
            public const string TurnOffName = "Turn off";
            public const string TurnOffDescription = "Turn off the machine if idle, no coffe will be surved !";
            public const int TurnOnId = 2;
            public const string TurnOnName = "Turn On";
            public const string TurnOnDescription = "Turing On the machine if off";
            public const int MakeCoffeeId = 3;
            public const string MakeCoffeeName = "Make Coffee";
            public const string MakeCoffeeDescription = "making coffe if idle (On, not making coffe and no alert)";
        }

        // Log messages
        public const string LogAddedDemoRecords = "Added 100 coffee for demo records";

        // Default values
        public const int DefaultEspressoShotsMin = 1;
        public const int DefaultEspressoShotsMax = 4;
        public const int DefaultDemoRecordsCount = 100;
        public const int DefaultRandomUpperBound = 2;
    }
}

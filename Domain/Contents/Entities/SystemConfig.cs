namespace Daco.Domain.Contents.Entities
{
    public class SystemConfig : Entity
    {
        public string          ConfigKey       { get; private set; } //'platform.commission_rate', 'order.auto_cancel_days'
        public string          ConfigValue     { get; private set; }
        public ConfigValueType ValueType       { get; private set; }
        public string?         Category        { get; private set; } //'platform', 'order', 'payment', 'shipping'
        public string?         Description     { get; private set; }
        public string?         ValidationRules { get; private set; }
        public bool            IsPublic        { get; private set; }
        public bool            IsEditable      { get; private set; }
        public Guid?           UpdatedBy       { get; private set; }
        public DateTime?       UpdatedAt       { get; private set; }
        public DateTime        CreatedAt       { get; private set; }

        protected SystemConfig() { }
    }
}

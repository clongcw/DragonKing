using SqlSugar;

namespace DragonKing.Database.EntityModel
{
    public class Privilege
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public int RoleId { get; set; }
        public bool Status { get; set; } = false;
        public string Name { get; set; }
        public string Visiual { get; set; } = "Visiual";
        public bool IsEnabled { get; set; } = false;
        public bool IsReadOnly { get; set; } = true;

        [Navigate(NavigateType.OneToOne, nameof(RoleId))]
        public Role Role { get; set; }
    }
}

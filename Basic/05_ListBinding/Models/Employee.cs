namespace ListBindingExample.Models
{
    /// <summary>
    /// 員工資料模型
    /// </summary>
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }
    }
}

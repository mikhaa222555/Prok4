using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Prok4
{
    internal class DateBase { }

namespace TaskManagerApp.Models
    {
        public class Клиент
        {
            [Key]
            public int ClientId { get; set; }
            public string Имя { get; set; }
            public string Email { get; set; }
            public string Телефон { get; set; }
            public byte[] ПарольHash { get; set; }
            public byte[] Salt { get; set; }
            public int ОтделId { get; set; }
            [ForeignKey("ОтделId")]
            public Отдел Отдел { get; set; }
            public DateTime ДатаРегистрации { get; set; }
            public byte[] ЗашифрованныеДанные { get; set; } // Зашифрованные данные (например, номер паспорта)
            public List<Задачи> Задачи { get; set; } = new List<Задачи>();
        }

        public class Задачи
        {
            [Key]
            public int TaskId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTime DueDate { get; set; }
            public int PriorityId { get; set; }
            [ForeignKey("PriorityId")]
            public Приоритеты Priority { get; set; }
            public int StatusId { get; set; }
            [ForeignKey("StatusId")]
            public Статусы Status { get; set; }
            public int ClientId { get; set; }
            [ForeignKey("ClientId")]
            public Клиент Client { get; set; }
            public DateTime ДатаСоздания { get; set; }
        }

        public class Отдел
        {
            [Key]
            public int ОтделId { get; set; }
            public string Название { get; set; }
            public string Описание { get; set; }
            public byte[] ЗашифрованныеДанные { get; set; }
            public List<Клиент> Клиенты { get; set; } = new List<Клиент>();
        }

        public class Приоритеты
        {
            [Key]
            public int PriorityId { get; set; }
            public string Name { get; set; }
            public List<Задачи> Задачи { get; set; } = new List<Задачи>();
        }

        public class Статусы
        {
            [Key]
            public int StatusId { get; set; }
            public string Name { get; set; }
            public List<Задачи> Задачи { get; set; } = new List<Задачи>();
        }
    }
}

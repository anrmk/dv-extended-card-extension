using System.Collections.Generic;

namespace ExtendedCardExtension.Models {
    public class ExtendedCardDataModel<T> {
        /// <summary>
        /// Состояние документа
        /// </summary>
        public string State { get; set; }
        
        /// <summary>
        /// Создал карточку
        /// </summary>
        public string CardRegistrarName { get; set; }

        /// <summary>
        /// Номер Документа
        /// </summary>
        public string NumberItemName { get; set; }

        /// <summary>
        /// Дата от
        /// </summary>
        public string CreateDate { get; set; }

        /// <summary>
        /// Инициатор (Заказчик)
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Текущий исполнитель
        /// </summary>
        public string CurrentPerformers { get; set; }

        /// <summary>
        /// Компания
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// Вид заявки
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// Краткое наименование
        /// </summary>
        public string SortName { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        // public DateTime Date { get; set; } = DateTime.Now;

        public List<T> ChildList { get; set; } = new List<T>();
    }

    
}
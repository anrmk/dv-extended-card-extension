using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExtendedCardExtension.Models {
    public class TaskDataModel {
        /// <summary>
        /// Идентификатор записи
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Вид задания
        /// </summary>
        public string Kind { get; set; }

        /// <summary>
        /// Автор
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Состояние
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Дата начала исполнения
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// Дата завершение плановая
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// Назначенный исполнитель
        /// </summary>
        public string AcquaintancePersonsName { get; set; }

        /// <summary>
        /// Назначенные исполнители
        /// 80C8F976-1F0D-4B67-82D2-A59230ECADE7 
        /// </summary>
        public string Performers { get; set; }

        /// <summary>
        /// Текущий исполнитель
        /// 134EA363-F5A8-4B80-B302-B21C954CE983 
        /// </summary>
        public string CurrentPerformers { get; set; }

        /// <summary>
        /// Трудоемкость плановая (ч)
        /// </summary>
        public int? Laboriousness { get; set; }

        /// <summary>
        /// Статус выполнения
        /// </summary>
        public string PercentCompleted { get; set; }
    }
}
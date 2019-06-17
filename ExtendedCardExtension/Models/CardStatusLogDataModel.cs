using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExtendedCardExtension.Models {
    /// <summary>
    /// Модель журнала перехода состояния
    /// </summary>
    public class CardStatusLogDataModel {
        /// <summary>
        /// Идетнификатор карты
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Идентификатор сотрудника
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// Имя сотрудника
        /// </summary>
        public string EmployeeName { get; set; }

        /// <summary>
        /// Дата начала этапа
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Дата окончания этапа
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// Переход в состояние
        /// </summary>
        public string WorkLabel { get; set; }

        /// <summary>
        /// Затрачено времени(трудозатраты)
        /// </summary>
        public string Labourness { get; set; }
    }
}
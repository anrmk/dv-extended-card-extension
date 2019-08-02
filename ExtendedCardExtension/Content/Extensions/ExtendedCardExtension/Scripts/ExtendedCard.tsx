namespace WebClient {
    export interface IExtendedCardDataModel<T> {
        cardRegistrarName: string;
        author: string;
        shortName: string;
        description: string;
        createDate: string;
        currentPerformers: string;
        state: string;
        partners: string;

        childList: Array<T>;
    }

    export interface ITaskDataModel {
        id: string;
        author: string;
        kind: string;
        state: string;
        endDate: string;
        completedUser: string;
        performers: string;
        currentPerformers: string;

        startDate: string;
        name: string;
        //Performers = task.CurrentPerformers?.FirstOrDefault(x => x.Employee?.DisplayName),
        laboriousness: string;
        percentCompleted: string;
    }

    export interface ICardStatusLogDataModel {
        id: string;
        employeeId: string;
        employeeName: string;
        date: string;
        endDate: string;
        workLabel: string;
        labourness: string;
    }

    export interface IReconciliationDataModel {
        id: string;
        employeeId: string;
        employeeName: string;
        date: string;
        endDate: string;
        workLabel: string;
        labourness: string;
    }

    export class ExtendedCard {
        constructor(...args) { }

        extendedCardTasksRender(data: IExtendedCardDataModel<ITaskDataModel>) {
            return (
                <div className="show-report-data">
                    <div><label>Тема: {data.shortName}</label></div>
                    <div><label>Автор: {data.author}</label></div>
                    <div><label>Текущий исполнитель: {data.currentPerformers}</label></div>
                    <div><label>Состояние документа: {data.state}</label></div>
                    <div>
                        <table>
                            <caption>Задания по документы:</caption>
                            <thead>
                                <tr>
                                    <th>Задание</th>
                                    <th>Вид</th>
                                    <th>Автор</th>
                                    <th>Состояние</th>
                                    <th>Дата завершения (фактическая)</th>
                                    <th>Назначенный исполнитель</th>
                                    <th>Текущий исполнитель</th>
                                </tr>
                            </thead>
                            <tbody>
                                {data.childList.map((value, index) => <tr key={index}><td><a href={`#/TaskView/${value.id}`} target='_blank'>{value.name}</a><br /><small>{value.id}</small></td><td>{value.kind}</td><td>{value.author}</td><td>{value.state}</td><td>{value.endDate}</td><td>{value.performers}</td><td>{value.currentPerformers}</td></tr>)}
                            </tbody>
                        </table>
                    </div>
                </div >
            );
        }

        /**
         * Получить информацию по заданиям
         * @param cardId - идентификатор документа
         */
        getExtendedCardTasks(cardId: string): JQueryDeferred<IExtendedCardDataModel<ITaskDataModel>> {
            let url = urlStore.urlResolver.resolveApiUrl("GetExtendedCardTasks", "ExtendedCard");
            return requestManager.get<IExtendedCardDataModel<ITaskDataModel>>(`${url}?cardId=${cardId}`);
        }

        /**
         * Лист согласования
         * @param data
         */
        extendedCardStatusLogsRender(data: IExtendedCardDataModel<ICardStatusLogDataModel>) {
            return (
                <div className="show-report-data">
                    <div><label>Название: {data.shortName}</label></div>
                    <div><label>Дата создания: {data.shortName}</label></div>
                    <div><label>Автор: {data.author}</label></div>
                    <div><label>Состояние документа: {data.state}</label></div>
                    <div>
                        <table>
                            <caption>Информация по переходу состояний:</caption>
                            <thead>
                                <tr>
                                    <th>Сотрудник</th>
                                    <th>Переход в состояние</th>
                                    <th>Начало этапа</th>
                                    <th>Окончание этапа</th>
                                    <th>Затрачено времени</th>
                                </tr>
                            </thead>
                            <tbody>
                                {data.childList.map((value, index) => <tr key={index}><td>{value.employeeName}</td><td>{value.workLabel}</td><td>{value.date}</td><td>{value.endDate}</td><td>{value.labourness}</td></tr>)}
                            </tbody>
                        </table>
                    </div>
                </div >
            );
        }

        extendedReconciliationListRender(data: IExtendedCardDataModel<IReconciliationDataModel>) {
            return (
                <div className="show-report-data">
                    <div><label>Наименование/предмет договора: {data.shortName}</label></div>
                    <div><label>Наименование объекта: {data.description}</label></div>
                    <div><label>Заказчик: {data.author}</label></div>
                    <div><label>Материалы приобретает: {data.state}</label></div>
                    <div><label>Стороны: {data.partners}</label></div>
                    <div><label>Сумма: {data.state}</label></div>
                    <div><label>Бюджет: {data.state}</label></div>
                    <div><label>Сметная стоимость: {data.state}</label></div>
                    <div><label>Порядок оплат: {data.state}</label></div>
                    <div><label>Срок выполнения работ: {data.state}</label></div>
                    <div><label>Гарантия: {data.state}</label></div>
                    <div><label>Примечания: {data.state}</label></div>
                    <div>
                        <table>
                            <caption>Список согласований и исполнений по документу:</caption>
                            <thead>
                                <tr>
                                    <th>№</th>
                                    <th>ФИО</th>
                                    <th>Управление</th>
                                    <th>Тип задания</th>
                                    <th>Дата начала</th>
                                    <th>Дата завершенная</th>
                                    <th>Решение</th>
                                    <th>Комментарий</th>
                                </tr>
                            </thead>
                            <tbody>
                                {data.childList.map((value, index) => <tr key={index}><td>{value.employeeName}</td><td>{value.workLabel}</td><td>{value.date}</td><td>{value.endDate}</td><td>{value.labourness}</td></tr>)}
                            </tbody>
                        </table>
                    </div>
                </div>
            );
        }

        /**
         * Журнал перехода состояний
         * @param cardId - идентификатор документа
         */
        getCardStatusLogs(cardId: string): JQueryDeferred<IExtendedCardDataModel<ICardStatusLogDataModel>> {
            let url = urlStore.urlResolver.resolveApiUrl("GetCardStatusLogs", "ExtendedCard");
            return requestManager.get<IExtendedCardDataModel<ICardStatusLogDataModel>>(`${url}?cardId=${cardId}`);
        }

        getReconciliationList(cardId: string): JQueryDeferred<IExtendedCardDataModel<IReconciliationDataModel>> {
            let url = urlStore.urlResolver.resolveApiUrl("GetCardStatusLogs", "ExtendedCard");
            return requestManager.get<IExtendedCardDataModel<ICardStatusLogDataModel>>(`${url}?cardId=${cardId}`);
        }

        getCardKindId(cardId: string): JQueryDeferred<string> {
            let url = urlStore.urlResolver.resolveApiUrl("GetCardKindId", "ExtendedCard");
            return requestManager.get(`${url}?cardId=${cardId}`);
        }

        getCardStatus(cardId: string): JQueryDeferred<string> {
            let url = urlStore.urlResolver.resolveApiUrl("GetCardStatus", "ExtendedCard");
            return requestManager.get(`${url}?cardId=${cardId}`);
        }
    }

    //$(() => {
    //    // Встраиваем метод showReportData в код, выполняемый при открытии карточки задания
    //    if (WebClient && WebClient.Cards && WebClient.Cards.TaskCardView && WebClient.Cards.TaskCardView.prototype['Initialize']) {
    //        const oldInitialize = WebClient.Cards.TaskCardView.prototype['Initialize'];
    //        WebClient.Cards.TaskCardView.prototype['Initialize'] = function (...args) {
    //            oldInitialize.apply(this, args);
    //            //showReportData();

    //            const cardId = $('.dv-card').data('cardId');
    //            if (!cardId) {
    //                return;
    //            }
    //            var srd = new WebClient.ExtendedCard();
    //            srd.getCardStatus(cardId).then((response) => {
    //                if (response != null) {
    //                    alert(response);
    //                }
    //            });

    //        }
    //    }
    //});
}

function getExtendedCardTasks(sender) {
    var layout = sender.layout;
    var srd = new WebClient.ExtendedCard();
    srd.getExtendedCardTasks(layout.cardInfo.id).done((response) => {
        if (response != null) {
            let element = srd.extendedCardTasksRender(response);
            WebClient.MessageBox.ShowInfo(element, `${response.shortName}`).done(() => {
                //alert("Диалог закрыт");
            });
        }
    })/*.fail((response) => {
        WebClient.MessageBox.ShowInfo(response.Message, "Ошибка").done(() => {
            //alert("Диалог закрыт");
        });
    })*/;
}

function getExtendedCardStatusLogs(sender) {
    var layout = sender.layout;
    var srd = new WebClient.ExtendedCard();
    srd.getCardStatusLogs(layout.cardInfo.id).then((response) => {
        if (response != null) {
            let element = srd.extendedCardStatusLogsRender(response);
            WebClient.MessageBox.ShowInfo(element, `${response.shortName}`).done(() => { });
        }
    });
}

function getCardReconciliationList(sender) {
    var layout = sender.layout;
    var srd = new WebClient.ExtendedCard();
    srd.getReconciliationList(layout.cardInfo.id).then((response) => {
        if (response != null) {
            let element = srd.extendedReconciliationListRender(response);
            WebClient.MessageBox.ShowInfo(element, `${response.shortName}`).done(() => { });
        }
    });
}
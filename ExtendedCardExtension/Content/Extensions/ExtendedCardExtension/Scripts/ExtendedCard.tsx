namespace WebClient {
    export interface IExtendedCardDataModel<T> {
        cardRegistrarName: string;
        author: string;
        sortName: string;
        description: string;
        createDate: string;
        currentPerformers: string;
        state: string;

        childList: Array<T>;
    }

    export interface ITaskDataModel {
        id: string;
        author: string;
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

    export class ExtendedCard {
        constructor(...args) { }

        extendedCardTasksRender(data: IExtendedCardDataModel<ITaskDataModel>) {
            return (
                <div className="show-report-data">
                    <div><label>Тема: {data.sortName}</label></div>
                    <div><label>Автор: {data.author}</label></div>
                    <div><label>Текущий исполнитель: {data.currentPerformers}</label></div>
                    <div><label>Состояние документа: {data.state}</label></div>
                    <div>
                        <table>
                            <caption>Задания по документы:</caption>
                            <thead>
                                <tr>
                                    <th>Ссылка на задание</th>
                                    <th>Автор</th>
                                    <th>Состояние</th>
                                    <th>Дата завершения (плановая)</th>
                                    <th>Назначенный исполнитель</th>
                                    <th>Текущий исполнитель</th>
                                </tr>
                            </thead>
                            <tbody>
                                {data.childList.map((value, index) => <tr key={index}><td><a href={`#/TaskView/${value.id}`} target='_blank' >{value.id}</a></td><td>{value.author}</td><td>{value.state}</td><td>{value.endDate}</td><td>{value.performers}</td><td>{value.currentPerformers}</td></tr>)}
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

        extendedCarStatusLogsRender(data: IExtendedCardDataModel<ICardStatusLogDataModel>) {
            return (
                <div className="show-report-data">
                    <div><label>Название: {data.sortName}</label></div>
                    <div><label>Дата создания: {data.sortName}</label></div>
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

        /**
         * Журнал перехода состояний
         * @param cardId - идентификатор документа
         */
        getCardStatusLogs(cardId: string): JQueryDeferred<IExtendedCardDataModel<ICardStatusLogDataModel>> {
            let url = urlStore.urlResolver.resolveApiUrl("GetCardStatusLogs", "ExtendedCard");
            return requestManager.get<IExtendedCardDataModel<ICardStatusLogDataModel>>(`${url}?cardId=${cardId}`);
        }

        getCardKindId(cardId: string): JQueryDeferred<string> {
            let url = urlStore.urlResolver.resolveApiUrl("GetCardKindId", "ExtendedCard");
            return requestManager.get(`${url}?cardId=${cardId}`);
        }
    }
}

function getExtendedCardTasks(sender) {
    var layout = sender.layout;
    var srd = new WebClient.ExtendedCard();
    srd.getExtendedCardTasks(layout.cardInfo.id).then((response) => {
        if (response != null) {
            let element = srd.extendedCardTasksRender(response);
            WebClient.MessageBox.ShowInfo(element, `${response.sortName}`).done(() => {
                //alert("Диалог закрыт");
            });
        }
    });
}

function getExtendedCardStatusLogs(sender) {
    var layout = sender.layout;
    var srd = new WebClient.ExtendedCard();
    srd.getCardStatusLogs(layout.cardInfo.id).then((response) => {
        if (response != null) {
            let element = srd.extendedCarStatusLogsRender(response);
            WebClient.MessageBox.ShowInfo(element, `${response.sortName}`).done(() => { });
        }
    });
}
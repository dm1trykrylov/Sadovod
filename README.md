# ЯМыСадовод
# Описание проекта

Почти у каждого есть знакомый или даже член семьи, который занимается садоводством или ведением собственного хозяйства, не так ли? Так вот, приложение, которое мы создали ориентированно именно на этот круг людей. Растения болеют? Не беда. Забиваешь основные симптомы, будь то пожелтение листьев или полное омертвление тканей, приложение выводит название болезни и похожие на нее, а также пути решения данной проблемы(в соответствии с популярными интернет ресурсами и т.д.). Также, на посадку растений влияют фазы луны. Зачем покупать лунный календарь или тратить лишнее время на просмотр информации в сети Интернет, если можно взять и увидеть это все в приложении. Это приложение подойдет даже юным огородникам, из-за прошлого приложения, а также возможности просмотра прогноза погоды с последующим составлением графика поливания ваших растений.

# Архитектура

Приложение состоит из трёх частей:
* Страницы с прогнозом погоды
* Календаря
* Сервиса поиска болезни растения

Страница **информация о погоде** предоставляет данные в реальном времени и прогноз на три дня. Источник информации - сайт [**openweathermap**](https://openweathermap.org). Получение данных:
1) приложение получает информацию о местоположении устройства
2) отправляется GET запрос к сайту-поставщику прогноза

**Календарь** отображает текущий месяц, каждая дата выделена цветом, соответствующим уровню благоприятности дня для садовых работ. Изначально все даты отмечены благоприятными. Особые даты получаются так:
* приложение отправляет GET запрос к Azure Function
  * Azure Function делает запрос к базе данных
  * Полученный список дат возвращается программе
* цвет ячеек календаря меняется в соответствии с полученным списком

**Поиск болезней** происходит на основе данных пользователя и также взаимодействует с Azure Function:
* пользователь вводит название растения
* отправляется GET запрос симптомов к Azure Function
  * Azure Function запрашивает в базе данных информацию о растении
  * Azure Function возвращает возможные симптомы для растения, если оно есть в базе данных
* пользователь выбирает симптомы из списка
* происходит отправка второго GET запроса к Azure Function
  * Azure Function запрашивает в базе данных наиболее подходящую болезнь по симптомам
  * Если болезнь найдена, возвращается её название и информация о ней и способ борьбы
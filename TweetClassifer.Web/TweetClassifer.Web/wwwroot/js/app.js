function connect() {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/tweetsClass")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    connection.on("Tweets", processTweets);

    connection.start()
        .then(() => console.log("Connected"))
        .catch(err => console.error(err.toString()));
}

function maleTweet(id) {
    let httpClient = new XMLHttpRequest();
    httpClient.open("GET", "/tweets/?id=" + id + "gender=0", true);
    httpClient.send();
} 

function femaleTweet(id) {
    let httpClient = new XMLHttpRequest();
    httpClient.open("GET", "/tweets/?id=" + id + "gender=1", true);
    httpClient.send();
}

function nonbinTweet(id) {
    let httpClient = new XMLHttpRequest();
    httpClient.open("GET", "/tweets/?id=" + id + "gender=2", true);
    httpClient.send();
}

function dismiss(id) {
    let httpClient = new XMLHttpRequest();
    httpClient.open("DELETE", "/tweets?id=" + id, true);
    httpClient.send();
}

function buildTweetHtml(tweet) {
    let opening = "<div>";
    let text = "<p>" + tweet.Tweet.Text + "</p>";
    let linkSection = "<a href=\"" + tweet.Tweet.URL + "\" target=\"_blank\">twitter</a>";
    let buttonMale = "<button type=\"button\" onclick=\"maleTweet(\"" + tweet.ID + "\");\">Male</button>";
    let buttonFemale = "<button type=\"button\" onclick=\"femaleTweet(\"" + tweet.ID + "\");\">Female</button>";
    let buttonNonBinary = "<button type=\"button\" onclick=\"nonbinTweet(\"" + tweet.ID + "\");\">No binary</button>";
    let dismissButton = "<button type=\"button\" onclick=\"dismiss(\"" + tweet.ID + "\");\">Dismiss</button>";
    let clossing = "</div>";

    let basicInfo = opening + text + linkSection + buttonMale + buttonFemale+ buttonNonBinary + dismissButton + clossing;
    return basicInfo;
}

function processTweets(tweets) {
    let parsedTweets = JSON.parse(tweets);
    let list = document.getElementById("tweetList");
    for (var i = 0; i < parsedTweets.length; i++) {
        var tweet = parsedTweets[i];
        console.log(tweet);
        let html = buildTweetHtml(tweet);
        list.innerHTML += "<li>" + html + "</li>";
    }
}

document.addEventListener("DOMContentLoaded", () => {
    connect();
});
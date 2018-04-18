from stravalib.client import Client
from flask import Flask, Response
import json

app = Flask(__name__)
client = Client()
client.access_token = "d7c39d6933831f833e61b52632e13bd7fd710de1" #d107

# client.access_token = "55d1f207d59c98d061a2016ed77225d9ebb9a2f4" #dmc77


# client.access_token = "bf356fe1baf6b9dde0d49fa70e7f84d2a647fdff" # yas
types = ['time', 'latlng', 'altitude', 'heartrate', 'temp', ]
activity_id = 1437408506
tim_activity_id = 1467628154
yas_activity_id = 1440429292

@app.route('/athlete')
def get_athlete():
    athlete = client.get_athlete()
    print(athlete)
    return "ok"

@app.route('/activity')
def get_activity():
    activity = client.get_activity(activity_id)
    print(activity)
    return "ok"

@app.route('/stream')
def get_stream():
    streams = client.get_activity_streams(activity_id, types=types, resolution='medium')
    print(streams)
    # latlng = []
    if 'latlng' in streams.keys():
        print(streams['latlng'].data)
        latlng = streams['latlng'].data
        print(len(latlng))
    return json.dumps(latlng)

@app.route('/altitude')
def get_altitude():
    streams = client.get_activity_streams(activity_id, types=types, resolution='medium')
    print(streams)
    if 'altitude' in streams.keys():
        altitudes = streams['altitude'].data
        print(streams['altitude'].data)
    return json.dumps(altitudes)

if __name__ == '__main__':
    # athlete = get_athlete()
    # activity = get_activity()
    # stream = get_stream()
    app.run(host="127.0.0.1", port=8010)

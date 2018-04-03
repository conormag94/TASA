from stravalib.client import Client
from flask import Flask, Response
import json

app = Flask(__name__)
client = Client()
client.access_token = "d7c39d6933831f833e61b52632e13bd7fd710de1"
types = ['time', 'latlng', 'altitude', 'heartrate', 'temp', ]

@app.route('/athlete')
def get_athlete():
    athlete = client.get_athlete()
    print(athlete)
    return "ok"

@app.route('/activity')
def get_activity():
    activity = client.get_activity(1437408506)
    print(activity)
    return "ok"

@app.route('/stream')
def get_stream():
    streams = client.get_activity_streams(1437408506, types=types, resolution='medium')
    print(streams)
    # latlng = []
    if 'latlng' in streams.keys():
        print(streams['latlng'].data)
        latlng = streams['latlng'].data
    if 'altitude' in streams.keys():
        print(streams['altitude'].data)
    return json.dumps(latlng)

if __name__ == '__main__':
    # athlete = get_athlete()
    # activity = get_activity()
    # stream = get_stream()
    app.run(host="127.0.0.1", port=8010)
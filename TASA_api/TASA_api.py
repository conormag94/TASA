from stravalib.client import Client
from flask import Flask, Response, request
import json
import config

app = Flask(__name__)
client = Client()

client.access_token = config.CYCLE_TOKEN

types = ['time', 'latlng', 'altitude', 'heartrate', 'temp', ]


# d_activity_id = 1437408506
# tim_activity_id = 1467628154
# yas_activity_id = 1440429292

def setUser(user):
    if (user == 'tim'):
        client.access_token = config.SKI_TOKEN
    elif (user == 'darragh'):
        client.access_token = config.DUBLIN_WALK_TOKEN
    elif (user == 'yas'):
        client.access_token = config.CYCLE_TOKEN


# Return information about logged in athlete
@app.route('/athlete')
def get_athlete():
    athlete = client.get_athlete()
    return athlete


@app.route('/stream')
def get_activities():
    activity_id = request.get('id')
    activity = client.get_activity(activity_id)
    print(activity)
    return "ok"


@app.route('/activity_list')
def get_activity_list():
    user = request.args.get('user')
    setUser(user)
    activity_limit = int(request.args.get('limit'))
    activity_request = client.get_activities(after="2010-01-01T00:00:00Z", limit=activity_limit)

    activity_list = []

    for activity in activity_request:
        activity_list.append({"id": activity.id, "name": activity.name, "date": activity.start_date.isoformat()})

    return json.dumps(activity_list)


@app.route('/activity')
def get_activity():
    user = request.args.get('user')
    setUser(user)
    activity_id = request.args.get('id')

    streams = client.get_activity_streams(activity_id, types=types, resolution='medium')
    print(streams)

    if 'latlng' in streams.keys():
        print(streams['latlng'].data)
        latlng = streams['latlng'].data
        print(len(latlng))
    return json.dumps(latlng)


@app.route('/altitude')
def get_altitude():
    user = request.args.get('user')
    setUser(user)

    activity_id = request.get('id')

    streams = client.get_activity_streams(activity_id, types=types, resolution='medium')
    print(streams)
    if 'altitude' in streams.keys():
        altitudes = streams['altitude'].data
        print(streams['altitude'].data)
    return json.dumps(altitudes)


if __name__ == '__main__':
    app.run(host="127.0.0.1", port=8010)
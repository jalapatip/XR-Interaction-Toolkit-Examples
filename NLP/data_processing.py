import re
import json

import pandas as pd
from tqdm import tqdm

from spatial_algorithm import *


def data_converted(data_path):
    df_dict = pd.read_csv(data_path, index_col=False).to_dict(orient="records")

    collected_data = []

    # head_pos, head_rotation, utterance, target_object, target_operation
    idx = 0
    for item in tqdm(df_dict):
        if item['gestureStatus'] == "completed" or item['gestureStatus'] == "started":
            head_pos = [item['headPosx'], item['headPosy'], item['headPosz']]
            head_rotation = [item['headRotx'], item['headRoty'], item['headRotz']]

            # TODO need to update for new version data => improve the efficiency
            if item['gestureStatus'] == "completed":
                utterance = item['utterance']
                if "microwave" in utterance or "MW" in utterance:
                    target_object = "microwave"
                else:
                    target_object = "oven"
                    # open the album
                    # codes the island
                    # find alan
                    # open the outlook
                    print(utterance)

                if "open" in utterance:
                    target_operation = "open"
                else:
                    target_operation = "close"
                    # codes the island
                    # find alan
                    print(utterance)

                collected_data.append(
                    [head_pos, head_rotation, "completed", utterance, target_object, target_operation])
                for data in collected_data:
                    if data[-3] == idx:
                        data[-3] = utterance
                    if data[-2] == idx:
                        data[-2] = target_object
                    if data[-1] == idx:
                        data[-1] = target_operation
                idx += 1
            else:
                collected_data.append([head_pos, head_rotation, "started", idx, idx, idx])

        new_df = pd.DataFrame(collected_data, columns=["head_pos", "head_rotation", "status", "utterance",
                                                       "target_object", "target_operation"])
        new_df.to_csv("data/converted_input.csv", index=None)


def main():
    # note: download the data into this folder

    data_path = "data/SmartHome Exp_2021-10-11-07-58-02.csv"
    # data_converted(data_path)

    df_dict = pd.read_csv("data/converted_input.csv", index_col=False).to_dict(orient="records")
    print(len(df_dict))

    # final data format
    # note: passing cosine_distance and euclidean_distance instead of gaze object list
    # [{cosine_distance: {}, euclidean_distance: {}, utterance: "", target_object: "", target_operation: ""}]

    with open("data/all_devices_1011.json", "r") as fin:
        all_devices = json.load(fin)

    all_completed_data = []
    for item in tqdm(df_dict):
        # if item["status"] == "completed":
        cosine_distance = {}
        euclidean_distance = {}
        for device in all_devices:
            distance_key = f'{device["instance_id"]}-{device["appliance_type"]}'
            head_pos = [float(pos) for pos in re.split(", |\[|\]|\(|\)", item["head_pos"]) if pos != '']
            head_rotation = [float(pos) for pos in re.split(", |\[|\]|\(|\)", item["head_rotation"]) if pos != '']
            equipment_pos = [float(pos) for pos in re.split(", |\[|\]|\(|\)", device["position"]) if pos != '']

            cosine_distance[distance_key] = get_cosine_distance(head_pos, rotation2direction(head_rotation),
                                                                equipment_pos)
            euclidean_distance[distance_key] = get_euclidean_distance(head_pos, equipment_pos)

        all_completed_data.append({
            "status": item["status"],
            "cosine_distance": cosine_distance,
            "euclidean_distance": euclidean_distance,
            "utterance": item["utterance"],
            "target_object": item["target_object"],
            "target_operation": item["target_operation"]
        })

    print(all_completed_data[0])
    print(len(all_completed_data))

    with open("data/input_data-all-1011.json", "w") as fout:
        json.dump(all_completed_data, fout, indent=4)


if __name__ == '__main__':
    main()
    # test = re.split(", |\[|\]|\(|\)", "(8.696393, 235.2555, 354.9054)")
    # data = [float(pos) for pos in test if pos != '']
    # print(data)

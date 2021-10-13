import numpy as np
from scipy.spatial.distance import euclidean, cosine


def get_cosine_distance(head_pos, gaze_direction, equipment_pos):
    # both positions follow the format V3 (x, y, z)
    heading = np.array(gaze_direction) - np.array(head_pos)
    equipment_direction = np.array(equipment_pos) - np.array(head_pos)

    # [0, 1] => in the front && [1, 2] => in the back

    # cosine distance (cosine distance = 1 - cosine similarity)
    # same as
    # 1 - np.dot(heading, equipment_direction.T) / (np.linalg.norm(heading) * np.linalg.norm(equipment_direction))
    # => smaller is closer
    return cosine(heading, equipment_direction)


def get_euclidean_distance(head_pos, equipment_pos):
    return euclidean(head_pos, equipment_pos)


def rotation2direction(rotation):
    # note: verified in Unity transform.rotation * Vector3.forward (0, 0, 1)
    # not the same results if use scipy.spatial.transform.Rotation
    x = rotation[0]
    y = rotation[1]

    x_1 = np.sin(np.deg2rad(y)) * np.cos(np.deg2rad(x))
    y_1 = np.sin(np.deg2rad(-x))
    z_1 = np.cos(np.deg2rad(x)) * np.cos(np.deg2rad(y))

    return [x_1, y_1, z_1]


if __name__ == '__main__':
    head_pos_test = [21.27763, 0.5008777, 0.7742695]
    head_rotation_test = [0.2547744, 350.4534, 303.9464]
    equipment_pos_test = [-2.5, 0.5, -21.0]

    print(get_cosine_distance(head_pos_test, rotation2direction(head_rotation_test), equipment_pos_test))
    print(get_euclidean_distance(head_pos_test, equipment_pos_test))

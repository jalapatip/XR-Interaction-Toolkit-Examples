import numpy as np


def get_cosine_distance(head_pos, gaze_pos, equipment_pos):
    # both positions follow the format (x, y, z)
    heading = np.array(gaze_pos) - np.array(head_pos)
    equipment_direction = np.array(equipment_pos) - np.array(head_pos)

    # only positive distance mean it is at the front
    return np.dot(heading, equipment_direction.T)


def get_euclidean_distance(head_pos, equipment_pos):
    return np.linalg.norm(head_pos, equipment_pos)


if __name__ == '__main__':
    # TODO 0.2547744, 350.4534, 303.9464
    cosine = get_cosine_distance([21.27763, 0.5008777, 0.7742695], [transform.forward.0.2547744, transform.forward.350.4534, transform.forward.303.9464], [-2.5, 0.5, -21.0])
    print(cosine)

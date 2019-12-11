import sys
import numpy as np
import cv2
from keras.models import load_model

model = load_model(sys.argv[2])
model.load_weights(sys.argv[3])

file_path = sys.argv[1]

img = cv2.imread(file_path)
gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
gray_flat = gray.reshape(1, 784)
prediction = np.argmax(model.predict(gray_flat,1))

print(prediction)
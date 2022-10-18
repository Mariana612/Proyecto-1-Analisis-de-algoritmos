import numpy as np
import matplotlib.pyplot as plt
from sklearn.preprocessing import StandardScaler
from numpy.random import uniform
from sklearn.datasets import make_blobs
import seaborn as sns
import random
import time


def euclidean(self,point, data):
    self.a += 1
    self.l += 1
    return np.sqrt(np.sum((point - data) ** 2, axis=1))


class KMeans:
    def __init__(self, n_clusters=8, max_iter=300):
        self.a = 0
        self.c = 0
        self.l = 0

        self.n_clusters = n_clusters
        self.max_iter = max_iter

    def fit(self, X_train):
        self.centroids = [random.choice(X_train)]

        for _ in range(self.n_clusters - 1):
            dists = np.sum([euclidean(self,centroid, X_train) for centroid in self.centroids], axis=0)
            dists /= np.sum(dists)
            new_centroid_idx, = np.random.choice(range(len(X_train)), size=1, p=dists)
            self.centroids += [X_train[new_centroid_idx]]

        iteration = 0
        prev_centroids = None

        while np.not_equal(self.centroids, prev_centroids).any() and iteration < self.max_iter:
            sorted_points = [[] for _ in range(self.n_clusters)]
            for x in X_train:
                dists = euclidean(self,x, self.centroids)
                centroid_idx = np.argmin(dists)
                sorted_points[centroid_idx].append(x)

            prev_centroids = self.centroids
            self.centroids = [np.mean(cluster, axis=0) for cluster in sorted_points]

            for i, centroid in enumerate(self.centroids):
                if np.isnan(centroid).any():  # Catch any np.nans, resulting from a centroid having no points
                    self.centroids[i] = prev_centroids[i]
            iteration += 1

    def evaluate(self, X):
        self.a += 2  # centroids,centroid_idxs
        self.l += 2  # centroids,centroid_idxs
        centroids = []
        centroid_idxs = []

        for x in X:
            self.a += 1  # assignation for each
            self.l += 1  # for each
            self.a += 2  # dists, centroid_idx
            self.l += 4  # dists, centroid_idx, 2 append

            dists = euclidean(self,x, self.centroids)
            centroid_idx = np.argmin(dists)
            centroids.append(self.centroids[centroid_idx])
            centroid_idxs.append(centroid_idx)
        self.l += 1  # return
        return centroids, centroid_idxs


# Create a dataset of 2D distributions
centers = 5
X_train, true_labels = make_blobs(n_samples=50000, centers=centers, random_state=42)
X_train = StandardScaler().fit_transform(X_train)
# Fit centroids to dataset


start_time = time.time()
kmeans = KMeans(n_clusters=centers)
kmeans.l+= 1
kmeans.fit(X_train)

print("--- %s seconds ---" % (time.time() - start_time))
# View results
class_centers, classification = kmeans.evaluate(X_train)
sns.scatterplot(x=[X[0] for X in X_train],
                y=[X[1] for X in X_train],
                hue=true_labels,
                style=classification,
                palette="deep",
                legend=None
                )
plt.plot([x for x, _ in kmeans.centroids],
         [y for _, y in kmeans.centroids],
         'k+',
         markersize=10,
         )
plt.show()

print(" a: " + str(kmeans.a) + " l: " + str(kmeans.l) + " c:  " + str(kmeans.c))

import tensorflow as  tf
x=tf.placeholder(tf.float32,shape=[],name='input')
y=tf.placeholder(tf.float32,shape=[],name='label')
w = tf.get_variable(
'weight', shape=[], initializer=tf.truncated_normal_initializer())
b = tf.get_variable('bias', shape=[], initializer=tf.zeros_initializer())
Y_predicted = w * x + b
loss = tf.square(y - Y_predicted, name='loss')
optimizer = tf.train.GradientDescentOptimizer(learning_rate=1e-3).minimize(loss)
init = tf.global_variables_initializer()
with tf.Session() as sess:
    writer = tf.summary.FileWriter('./linear_log', graph=sess.graph)
    sess.run(init)
    data=[[3,4],[5,6],[8,10]]
    print(sess.run(w) )
    print(sess.run(b) )
    tf.summary.scalar("loss", loss)
    merged_summary = tf.summary.merge_all()
    for i in range(100):
        total_loss = 0
        for a, c in data:
            _, l = sess.run([optimizer, loss], feed_dict={x: a, y: c})
            total_loss+=l
        summary=sess.run(merged_summary,feed_dict={x:3,y:4})
        print(summary)
        writer.add_summary(summary,i)
        writer.flush()
        print("Epoch {0}: {1}".format(i, total_loss / 2))
        print(sess.run(w))
        print(sess.run(b))
alph = {'a': 0, 'b': 1, 'c': 2, 'd': 3, 'e': 4, 'f': 5, 
		'g': 6, 'h': 7, 'i': 8, 'j': 9, 'k':10, 'l':11,
		'm':12, 'n':13, 'o':14, 'p':15, 'q':16, 'r':17,
		's':18, 't':19, 'u':20, 'v':21, 'w':22, 'x':23,
		'y':24, 'z':25, '\r':26, "'":27, '\n':28} 



def get_let_stats(x):
	fil = open('wordsEn.txt', 'r')
	stats = [0.0]*29
	for i in fil:
		for j in xrange(len(i)):
			if x == i[j]:
				stats[alph[i[j+1]]] = stats[alph[i[j+1]]] + 1.0
	return stats





bet = 'abcdefghijklmnopqrstuvwxyz'
all_stats = []
for let in bet:
	all_stats.append(get_let_stats(let))

for x in xrange(len(all_stats)):
	all_stats[x].pop(28)
	all_stats[x].pop(27)
	all_stats[x].pop(26)
	
	



percents = [[] for x in xrange(26)]
count = 0
for x in all_stats:
	s = float(sum(x))
	for y in x:
		percents[count].append(float(y/s))
	count = count + 1

f = open('wordstats.txt', 'w')

for x in xrange(26):
	f.write(bet[x])
	f.write('\n')
	for i in percents[x]:
		
		f.write(('%.10f ' % i))
	f.write('\n')